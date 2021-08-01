param(
  [string] $ImageTag = "latest",

  [Parameter(Mandatory = $true)]    
  [ValidateSet("dev", "staging")]
  [string] $TargetCloudEnvPrefix = "staging",

  [string] $ContainerRepository
)
$scripts_dir = "$PSScriptRoot\..\..\..\..\scripts"
Import-Module "${scripts_dir}\modules\Require-EnvironmentVariables.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$required_env_variables = @(
  "ESZOP_AZURE_EVENTBUS_CONN_STR",
  "ESZOP_SQLSERVER_CONN_STR_ORDERS"
)

Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$logs_dir = $env:ESZOP_LOGS_DIR
if (-not($logs_dir)) {
  $logs_dir = "/logs"
}

$global_config = Get-GlobalConfig
$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $global_config.AZ_CONTAINER_REPO }

docker run `
  --rm `
  -itd `
  -p 8000:80 `
  -p 8001:8080 `
  -e ASPNETCORE_URLS='http://+' `
  -e ESZOP_LOGS_DIR="$logs_dir" `
  -e ASPNETCORE_ENVIRONMENT="DevDockerCloud" `
  -e ESZOP_AZURE_EVENTBUS_CONN_STR="$env:ESZOP_AZURE_EVENTBUS_CONN_STR" `
  -e ESZOP_SQLSERVER_CONN_STR="$env:ESZOP_SQLSERVER_CONN_STR_ORDERS" `
  -e ESZOP_AZURE_EVENTBUS_TOPIC_NAME="eszop-${TargetCloudEnvPrefix}-event-bus-topic" `
  -e ESZOP_AZURE_EVENTBUS_SUB_NAME="eszop-${TargetCloudEnvPrefix}-event-bus-orders-sub" `
  -v "$pwd\..\..\..\logs:/logs" `
  --network eszop-network `
  --name eszop-orders-api `
  "${container_repo}/eszop-orders-api:$ImageTag"