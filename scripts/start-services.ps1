param(
    [string] $ImageTag = "latest"
)

Import-Module $PSScriptRoot\modules\Resolve-ServiceLocation.psm1 -Force

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
    $service_dir = Resolve-ServiceLocation -ServiceName $service
    $scripts_dir = Join-Path (Resolve-Path $service_dir) "scripts"
    $start_script = Join-Path $scripts_dir "start-service.ps1"

    if(Test-Path $start_script) {
        Write-Host "Run $start_script"
        & $start_script -ImageTag $ImageTag
    }
}