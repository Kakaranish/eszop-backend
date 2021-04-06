param(
    [string] $ImageTag = "latest"
)

Import-Module $PSScriptRoot\..\..\..\scripts\modules\Require-EnvironmentVariables.psm1 -Force -DisableNameChecking

$required_env_variables = @(
    "ASPNETCORE_ENVIRONMENT",
    "ESZOP_CLIENT_URI"
)

Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$logs_dir = $env:ESZOP_LOGS_DIR
if(-not($logs_dir)) {
    $logs_dir = "/logs"
}

docker run `
    --rm `
    -itd `
    -p 10000:80 `
    -e ASPNETCORE_ENVIRONMENT="$env:ASPNETCORE_ENVIRONMENT" `
    -e ESZOP_CLIENT_URI="$env:ESZOP_CLIENT_URI" `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$logs_dir" `
    -v "$pwd\..\..\logs:/logs" `
    --network eszop-network `
    --name eszop-api-gateway `
    "eszopregistry.azurecr.io/eszop-api-gateway:$ImageTag"
