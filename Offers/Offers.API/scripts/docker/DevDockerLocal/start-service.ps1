param(
  [string] $ImageTag = "latest",
  [string] $ContainerRepository
)

$scripts_dir = "$PSScriptRoot\..\..\..\..\scripts"
Import-Module "${scripts_dir}\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$global_config = Get-GlobalConfig
$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $global_config.AZ_CONTAINER_REPO }

docker run `
  --rm `
  -itd `
  -p 5000:80 `
  -p 5001:8080 `
  -e ASPNETCORE_URLS='http://+' `
  -e ESZOP_LOGS_DIR="/logs" `
  -e ASPNETCORE_ENVIRONMENT="DevDockerLocal" `
  -v "$pwd\..\..\..\logs:/logs" `
  --network eszop-network `
  --name eszop-offers-api `
  "${container_repo}/eszop-offers-api:$ImageTag"