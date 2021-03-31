Import-Module $PSScriptRoot\..\modules\Prepare-SqlConnectionString.psm1 -Force -DisableNameChecking
Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force

# ---  FILL VALUES BELOW vvv  --------------------------------------------------

$db_username = ""
$db_password = ""

$env:ASPNETCORE_ENVIRONMENT = "Staging"
$env:ESZOP_AZURE_STORAGE_CONN_STR = ""
$env:ESZOP_AZURE_EVENTBUS_CONN_STR = ""
$env:ESZOP_REDIS_CONN_STR = ""

# ------------------------------------------------------------------------------

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