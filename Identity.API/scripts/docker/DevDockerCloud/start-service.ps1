param(
    [string] $ImageTag = "latest",

    [Parameter(Mandatory = $true)]    
    [ValidateSet("dev", "staging")]
    [string] $TargetCloudEnvPrefix = "staging",

    [string] $ContainerRepository
)
$scripts_dir = "$PSScriptRoot\..\..\..\..\scripts"
Import-Module "${scripts_dir}\modules\Require-EnvironmentVariables.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\AzureConfig.psm1" -Force

$required_env_variables = @(
    "ASPNETCORE_ENVIRONMENT",
    "ESZOP_AZURE_EVENTBUS_CONN_STR",
    "ESZOP_SQLSERVER_CONN_STR_IDENTITY",
    "ESZOP_REDIS_CONN_STR"
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
    -p 6000:80 `
    -p 6001:8080 `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$logs_dir" `
    -e ASPNETCORE_ENVIRONMENT="$env:ASPNETCORE_ENVIRONMENT" `
    -e ESZOP_AZURE_EVENTBUS_CONN_STR="$env:ESZOP_AZURE_EVENTBUS_CONN_STR" `
    -e ESZOP_SQLSERVER_CONN_STR="$env:ESZOP_SQLSERVER_CONN_STR_IDENTITY" `
    -e ESZOP_REDIS_CONN_STR="$env:ESZOP_REDIS_CONN_STR" `
    -e ESZOP_AZURE_EVENTBUS_TOPIC_NAME="eszop-${TargetCloudEnvPrefix}-event-bus-topic" `
    -e ESZOP_AZURE_EVENTBUS_SUB_NAME="eszop-${TargetCloudEnvPrefix}-event-bus-identity-sub" `
    -v "$pwd\..\..\..\logs:/logs" `
    --network eszop-network `
    --name eszop-identity-api `
    "${container_repo}/eszop-identity-api:$ImageTag"