# ------------------------------------------------------------------------------
#
# This script builds all services and creates docker images.
# Built docker images are targeted for specific AzureContainerRepository that
# can be specified either in /config/global.yaml or as script argument.
# When "$ContainerRepository" is not present, value from /config/global.yaml
# is taken.
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
    $scripts_dir = Join-Path (Resolve-Path $service_dir) "scripts" "docker"
    $build_script = Join-Path $scripts_dir "build.ps1"

    if (Test-Path $build_script) {
        Write-Host "Run $build_script"
        & $build_script -ImageTag $ImageTag -ContainerRepository $container_repo
    }
}