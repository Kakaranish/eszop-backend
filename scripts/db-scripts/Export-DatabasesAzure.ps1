param (
  [Parameter(Mandatory = $true)]
  [ValidateSet("dev", "staging", "prod")]
  [string] $CloudEnv,

  [switch] $AutoApprove
)

Import-Module "$PSScriptRoot\..\modules\Get-GlobalConfig.psm1" -Force
Import-Module "$PSScriptRoot\..\modules\Get-AppsConfig.psm1" -Force

# ------------------------------------------------------------------------------

$apps_config = Get-AppsConfig -CloudEnv $CloudEnv
$global_config = Get-GlobalConfig

if (-not($AutoApprove.IsPresent)) {
  Write-Host "[INFO] You're going to commision export databases on '$CloudEnv' environment" -ForegroundColor Green
  $choice = Read-Host "Do you want to continue (y/n)"
  if ($choice -ne "y") {
    exit
  }
}

$services = @("offers", "identity", "carts", "orders", "notification")
$storage_account_key = $(Get-AzStorageAccountKey `
    -ResourceGroupName $global_config.AZ_RESOURCE_GROUP `
    -StorageAccountName $global_config.AZ_GLOBAL_STORAGE_NAME).Value[0]

$now_iso = Get-Date -UFormat '+%Y_%m_%dT%H_%M'

foreach ($service in $services) {
  $backup_filename = "eszop-$CloudEnv-$service-db-backup-$now_iso.bacpac"
  $server_name = "eszop-$CloudEnv-sqlserver"
  $db_name = "eszop-$CloudEnv-$service-db"

  New-AzSqlDatabaseExport `
    -ResourceGroupName "eszop-$CloudEnv" `
    -ServerName  $server_name `
    -DatabaseName $db_name `
    -StorageKeytype "StorageAccessKey" `
    -StorageKey $storage_account_key `
    -StorageUri "https://eszopstorage.blob.core.windows.net/eszop-db-backups/$backup_filename" `
    -AdministratorLogin $apps_config.SQLSERVER_USERNAME `
    -AdministratorLoginPassword (ConvertTo-SecureString $apps_config.SQLSERVER_PASSWORD -AsPlainText)

  Write-Host "[INFO] Commisioned export for $db_name" -ForegroundColor Green
}