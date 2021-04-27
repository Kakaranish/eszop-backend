param (
    [Parameter(Mandatory = $true)]
    [string[]] $BackupPrefixes
)

Import-Module "$PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1" -Force
Import-Module "$PSScriptRoot\..\modules\Require-CloudEnvVar.psm1" -Force -DisableNameChecking -Scope Local
Import-Module "$PSScriptRoot\..\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$cloud_env = Require-CloudEnvVar
$global_config = Get-GlobalConfig
$services = @("offers", "identity", "carts", "orders", "notification")

$storage_context = (Get-AzStorageAccount `
        -ResourceGroupName $global_config.AZ_RESOURCE_GROUP `
        -Name $global_config.AZ_GLOBAL_STORAGE_NAME).Context

Write-Host $storage_context.ConnectionString

$blob_name_template = "eszop-$cloud_env-{service_name}-db-backup-{backup_prefix}.bacpac"

foreach ($backup_prefix in $BackupPrefixes) {
    foreach ($service in $services) {
        $blob_name = $blob_name_template `
            -replace "{service_name}", $service `
            -replace "{backup_prefix}", $backup_prefix
                
        Remove-AzStorageBlob `
            -Container $global_config.AZ_BACKUPS_CONTAINER_NAME `
            -Blob $blob_name `
            -Context $storage_context
        
        Write-Host "[REMOVED] $blob_name"
    }
}