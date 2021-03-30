param(
    [string] $ImageTag = "latest"
)

Import-Module $PSScriptRoot\modules\Resolve-ServiceLocation.psm1 -Force

if(-not($env:ESZOP_AZURE_EVENTBUS_CONN_STR)) {
    Write-Error 'Env variable ESZOP_AZURE_EVENTBUS_CONN_STR is not set' -ErrorAction Stop
}

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