param(
    [string] $ImageTag = "latest"
)

Import-Module "$PSScriptRoot\..\..\..\scripts\modules\Require-EnvironmentVariables.psm1" -Force -DisableNameChecking
Import-Module "$PSScriptRoot\..\..\..\scripts\AzureConfig.psm1" -Force

$required_env_variables = @(
    "ASPNETCORE_ENVIRONMENT",
    "ESZOP_CLIENT_URI"
)

Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$logs_dir = $env:ESZOP_LOGS_DIR
if (-not($logs_dir)) {
    $logs_dir = "/logs"
}

$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $ESZOP_AZURE_CONTAINER_REPO }

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
    "${container_repo}/eszop-api-gateway:$ImageTag"
