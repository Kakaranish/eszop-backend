param (
  [Parameter(Mandatory = $true)]
  [string[]] $BackupPrefixes,

  [Parameter(Mandatory = $true)]
  [ValidateSet("dev", "staging", "prod")]
  [string] $CloudEnv
)

Import-Module "$PSScriptRoot\..\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$global_config = Get-GlobalConfig
$services = @("offers", "identity", "carts", "orders", "notification")

$storage_context = (Get-AzStorageAccount `
    -ResourceGroupName $global_config.AZ_RESOURCE_GROUP `
    -Name $global_config.AZ_GLOBAL_STORAGE_NAME).Context

$blob_name_template = "eszop-$CloudEnv-{service_name}-db-backup-{backup_prefix}.bacpac"

foreach ($backup_prefix in $BackupPrefixes) {
  foreach ($service in $services) {
    $blob_name = $blob_name_template `
      -replace "{service_name}", $service `
      -replace "{backup_prefix}", $backup_prefix
                
    Remove-AzStorageBlob `
      -Container $global_config.AZ_BACKUPS_CONTAINER_NAME `
      -Blob $blob_name `
      -Context $storage_context
        
    Write-Host "[INFO] Removed $blob_name"-ForegroundColor Green
  }
}