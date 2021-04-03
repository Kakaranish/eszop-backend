param (
    [Parameter(Mandatory = $true)]
    [string[]] $BackupPrefixes
)

Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Require-EnvironmentVariables.psm1 -Force -DisableNameChecking

$required_env_variables = @( "ASPNETCORE_ENVIRONMENT" )
Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$env_prefix = Resolve-EnvPrefix -Environment $env:ASPNETCORE_ENVIRONMENT
$services = @("offers", "identity", "carts", "orders", "notification")

$storage_context = (Get-AzStorageAccount `
    -ResourceGroupName "eszop" `
    -Name "eszopstorage").Context

$blob_name_template = "eszop-$env_prefix-{service_name}-db-backup-{backup_prefix}.bacpac"
$backups_container_name = "eszop-db-backups"

foreach ($backup_prefix in $BackupPrefixes) {
    foreach ($service in $services) {
        $blob_name = $blob_name_template `
            -replace "{service_name}", $service `
            -replace "{backup_prefix}", $backup_prefix
                
        Remove-AzStorageBlob -Container $backups_container_name -Blob $blob_name -Context $storage_context
        
        Write-Host "[REMOVED] $blob_name"
    }
}