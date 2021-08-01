# ------------------------------------------------------------------------------
#
# This script publishes docker images built with Build-Images.ps1
# These images are targeted for specific Azure Container Repository.
# If "$ContainerRepository" is not present then value from config/global.yaml
# is taken.
#
# Sample script call:
# $ .\Publish-Images.ps1 -ImageTag latest
#
# ------------------------------------------------------------------------------

param(
  [string] $ImageTag = "latest",
  [string] $ContainerRepository
)

$scripts_dir = "$PSScriptRoot\..\.."
Import-Module "${scripts_dir}\modules\Resolve-ServiceLocation.psm1" -Force
Import-Module "${scripts_dir}\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$global_config = Get-GlobalConfig
$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $global_config.AZ_CONTAINER_REPO }

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
  $service_dir = Resolve-ServiceLocation -ServiceName $service
  $scripts_dir = Join-Path (Resolve-Path $service_dir) ".." "scripts" "docker"
  $publish_script = Join-Path $scripts_dir "publish.ps1"

  if (Test-Path $publish_script) {
    Write-Host "Run $publish_script"
    & $publish_script -ImageTag $ImageTag -ContainerRepository $container_repo
  }
}