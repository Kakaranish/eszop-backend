param(
    [string] $ImageTag = "latest"
)

Import-Module $PSScriptRoot\modules\Resolve-ServiceLocation.psm1 -Force

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
    $service_dir = Resolve-ServiceLocation -ServiceName $service
    $scripts_dir = Join-Path (Resolve-Path $service_dir) "scripts"
    $build_script = Join-Path $scripts_dir "build.ps1"

    if(Test-Path $build_script) {
        Write-Host "Run $build_script"
        & $build_script -ImageTag $ImageTag
    }
}