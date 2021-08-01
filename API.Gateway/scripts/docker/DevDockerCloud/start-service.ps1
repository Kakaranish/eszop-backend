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
  "ESZOP_CLIENT_URI"
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
  -p 10000:80 `
  -e ASPNETCORE_ENVIRONMENT="DevDockerCloud" `
  -e ESZOP_CLIENT_URI="$env:ESZOP_CLIENT_URI" `
  -e ASPNETCORE_URLS='http://+' `
  -e ESZOP_LOGS_DIR="$logs_dir" `
  -v "$pwd\..\..\..\logs:/logs" `
  --network eszop-network `
  --name eszop-api-gateway `
  "${container_repo}/eszop-api-gateway:$ImageTag"
