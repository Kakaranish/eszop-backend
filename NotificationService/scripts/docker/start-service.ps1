param(
    [string] $ImageTag = "latest"
)

Import-Module "$PSScriptRoot\..\..\..\scripts\modules\Require-EnvironmentVariables.psm1" -Force -DisableNameChecking
Import-Module "$PSScriptRoot\..\..\..\scripts\AzureConfig.psm1" -Force

$required_env_variables = @(
    "ASPNETCORE_ENVIRONMENT",
    "ESZOP_AZURE_EVENTBUS_CONN_STR",
    "ESZOP_SQLSERVER_CONN_STR"
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
    -p 9000:80 `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$logs_dir" `
    -e ASPNETCORE_ENVIRONMENT="$env:ASPNETCORE_ENVIRONMENT" `
    -e ESZOP_AZURE_EVENTBUS_CONN_STR="$env:ESZOP_AZURE_EVENTBUS_CONN_STR" `
    -e ESZOP_SQLSERVER_CONN_STR="$env:ESZOP_SQLSERVER_CONN_STR_NOTIFICATION" `
    -v "$pwd\..\..\logs:/logs" `
    --network eszop-network `
    --name eszop-notification-service `
    "${container_repo}/eszop-notification-service:$ImageTag"