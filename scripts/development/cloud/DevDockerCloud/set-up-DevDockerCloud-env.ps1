# ------------------------------------------------------------------------------
# This script sets environment variables that are required by "start-services.ps1"
# ------------------------------------------------------------------------------

$scripts_dir = "$PSScriptRoot\..\..\..\"
Import-Module "${scripts_dir}\modules\Prepare-SqlConnectionString.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\modules\Resolve-EnvPrefix.psm1" -Force
Import-Module "${scripts_dir}\AppsConfig.psm1" -Force

$env:ASPNETCORE_ENVIRONMENT = "DevDockerCloud"
$env_prefix = Resolve-EnvPrefix -Environment $env:ASPNETCORE_ENVIRONMENT
$sql_conn_str_name_prefix = "ESZOP_SQLSERVER_CONN_STR_"

$services = @("offers", "identity", "carts", "orders", "notification")
foreach ($service in $services) {
    $conn_str = Prepare-SqlConnectionString `
        -EnvironmentPrefix $env_prefix `
        -ServiceName $service `
        -DbUsername $db_username `
        -DbPassword $db_password
    $service_name_uppercase = $service.ToUpperInvariant()
    $sql_conn_str_name = "$sql_conn_str_name_prefix$service_name_uppercase"

    Invoke-Expression ('$env:' + "$sql_conn_str_name=`"$conn_str`"")
}

$env:ESZOP_CLIENT_URI = "http://localhost:3000"
$env:ESZOP_AZURE_EVENTBUS_CONN_STR = $ESZOP_AZURE_EVENTBUS_CONN_STR
$env:ESZOP_AZURE_STORAGE_CONN_STR = $ESZOP_AZURE_STORAGE_CONN_STR
$env:ESZOP_REDIS_CONN_STR = $ESZOP_REDIS_CONN_STR

Write-Host "[INFO] DevDockerCloud env setup succeeded" -ForegroundColor Green