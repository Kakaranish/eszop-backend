param(
    [string] $ImageTag = "latest"
)

Import-Module $PSScriptRoot\modules\Resolve-ServiceLocation.psm1 -Force

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")


foreach ($service in $services) {
    $service_dir = Resolve-ServiceLocation -ServiceName $service
    $scripts_dir = Join-Path (Resolve-Path $service_dir) "scripts"
    $publish_scripts = Join-Path $scripts_dir "publish.ps1"

    if (Test-Path $publish_scripts) {
        Write-Host "Run $publish_scripts"
        & $publish_scripts -ImageTag $ImageTag
    }
}