param(
  [string] $ImageTag = "latest",
  [string] $ContainerRepository
)

$scripts_dir = "$PSScriptRoot\..\..\..\scripts"
Import-Module "${scripts_dir}\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$global_config = Get-GlobalConfig
$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $global_config.AZ_CONTAINER_REPO }

docker build -f $PSScriptRoot/../../Carts.API/Dockerfile -t "${container_repo}/eszop-carts-api:$ImageTag" $PSScriptRoot/../../..