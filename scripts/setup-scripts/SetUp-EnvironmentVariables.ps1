$scripts_dir = "$PSScriptRoot\.."

Import-Module "${scripts_dir}\modules\Get-AppsConfig.psm1" -Force -DisableNameChecking -Scope Local
Import-Module "${scripts_dir}\modules\Get-GlobalConfig.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\modules\Get-RequiredEnvPrefix.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\modules\Prepare-SqlConnectionString.psm1" -Force -DisableNameChecking

# ------------------------------------------------------------------------------

$apps_config = Get-AppsConfig
$env_prefix = Get-RequiredEnvPrefix

$sql_conn_str_name_prefix = "ESZOP_SQLSERVER_CONN_STR_"

$services = @("offers", "identity", "carts", "orders", "notification")
foreach ($service in $services) {
    $conn_str = Prepare-SqlConnectionString `
        -EnvironmentPrefix $env_prefix `
        -ServiceName $service `
        -DbUsername $apps_config.SQLSERVER_USERNAME `
        -DbPassword $apps_config.SQLSERVER_PASSWORD
    $service_name_uppercase = $service.ToUpperInvariant()
    $sql_conn_str_name = "${sql_conn_str_name_prefix}${service_name_uppercase}"

    Invoke-Expression ('$env:' + "$sql_conn_str_name=`"$conn_str`"")
}

$env:ESZOP_CLIENT_URI = "http://localhost:3000"
$env:ESZOP_AZURE_EVENTBUS_CONN_STR = $apps_config.ESZOP_AZURE_EVENTBUS_CONN_STR
$env:ESZOP_AZURE_STORAGE_CONN_STR = $apps_config.ESZOP_AZURE_STORAGE_CONN_STR
$env:ESZOP_REDIS_CONN_STR = "$($apps_config.REDIS_ADDRESS):$($apps_config.REDIS_PORT),password=$($apps_config.REDIS_PASSWORD)"

Write-Host "[INFO] Environment variables set for '${env_prefix}'" -ForegroundColor Green