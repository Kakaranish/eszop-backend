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
  -p 10000:80 `
  -e ASPNETCORE_URLS='http://+' `
  -e ASPNETCORE_ENVIRONMENT="DevDockerLocal" `
  -e ESZOP_LOGS_DIR="/logs" `
  -e ESZOP_CLIENT_URI="http://frontend" `
  -v "$pwd\..\..\..\logs:/logs" `
  --network eszop-network `
  --name eszop-api-gateway `
  "${container_repo}/eszop-api-gateway:$ImageTag"
