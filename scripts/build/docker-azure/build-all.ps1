param(
    [string] $ImageTag = "latest",
    [string] $ContainerRepository
)

Import-Module "$PSScriptRoot\..\..\modules\Resolve-ServiceLocation.psm1" -Force
Import-Module "$PSScriptRoot\..\..\AzureConfig.psm1" -Force

$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $ESZOP_AZURE_CONTAINER_REPO }
$services = @("gateway", "carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
    $service_dir = Resolve-ServiceLocation -ServiceName $service
    $scripts_dir = Join-Path (Resolve-Path $service_dir) "scripts" "docker"
    $build_script = Join-Path $scripts_dir "build.ps1"

    if(Test-Path $build_script) {
        Write-Host "Run $build_script"
        & $build_script -ImageTag $ImageTag -ContainerRepository $container_repo
    }
}