param(
    [string] $ImageTag = "latest"
)

Import-Module $PSScriptRoot\..\..\..\scripts\modules\Require-EnvironmentVariables.psm1 -Force -DisableNameChecking

$required_env_variables = @(
    "ASPNETCORE_ENVIRONMENT",
    "ESZOP_AZURE_EVENTBUS_CONN_STR",
    "ESZOP_SQLSERVER_CONN_STR",
    "ESZOP_AZURE_STORAGE_CONN_STR"
)

Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$logs_dir = $env:ESZOP_LOGS_DIR
if (-not($logs_dir)) {
    $logs_dir = "/logs"
}

docker run `
    --rm `
    -itd `
    -p 5000:80 `
    -p 5001:8080 `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$logs_dir" `
    -e ASPNETCORE_ENVIRONMENT="$env:ASPNETCORE_ENVIRONMENT" `
    -e ESZOP_AZURE_EVENTBUS_CONN_STR="$env:ESZOP_AZURE_EVENTBUS_CONN_STR" `
    -e ESZOP_SQLSERVER_CONN_STR="$env:ESZOP_SQLSERVER_CONN_STR_OFFERS" `
    -e ESZOP_AZURE_STORAGE_CONN_STR="$env:ESZOP_AZURE_STORAGE_CONN_STR" `
    -v "$pwd\..\..\logs:/logs" `
    --network eszop-network `
    --name eszop-offers-api `
    "eszopregistry.azurecr.io/eszop-offers-api:$ImageTag"