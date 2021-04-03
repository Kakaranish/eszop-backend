param (
    [Parameter(Mandatory = $true)]
    [string] $DbUsername,

    [Parameter(Mandatory = $true)]
    [string] $DbPassword,

    [switch] $AutoApprove
)

Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Require-EnvironmentVariables.psm1 -Force -DisableNameChecking

$required_env_variables = @( "ASPNETCORE_ENVIRONMENT" )
Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

if (-not($AutoApprove.IsPresent)) {
    Write-Host "You're going to commision export databases on $env:ASPNETCORE_ENVIRONMENT environment"
    $choice = Read-Host "Do you want to continue (y/n)"
    if ($choice -ne "y") {
        exit
    }
}

$environment_prefix = Resolve-EnvPrefix -Environment $env:ASPNETCORE_ENVIRONMENT
$services = @("offers", "identity", "carts", "orders", "notification")
$db_sa_password = (ConvertTo-SecureString $DbPassword -AsPlainText)
$storage_account_key = $(Get-AzStorageAccountKey `
        -ResourceGroupName "eszop" `
        -StorageAccountName "eszopstorage").Value[0]

$now_iso = Get-Date -UFormat '+%Y_%m_%dT%H_%M'

foreach ($service in $services) {
    $backup_filename = "eszop-$environment_prefix-$service-db-backup-$now_iso.bacpac"
    $server_name = "eszop-$environment_prefix-sqlserver"
    $db_name = "eszop-$environment_prefix-$service-db"

    New-AzSqlDatabaseExport `
        -ResourceGroupName "eszop-$environment_prefix" `
        -ServerName  $server_name `
        -DatabaseName $db_name `
        -StorageKeytype "StorageAccessKey" `
        -StorageKey $storage_account_key `
        -StorageUri "https://eszopstorage.blob.core.windows.net/eszop-db-backups/$backup_filename" `
        -AdministratorLogin $DbUsername `
        -AdministratorLoginPassword $db_sa_password

    Write-Host "Commisioned export for $db_name"
}