param(
    [string] $ImageTag = "latest"
)

Import-Module $PSScriptRoot\..\modules\Resolve-ServiceLocation.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Require-EnvironmentVariables.psm1 -Force -DisableNameChecking

$required_env_variables = @(
    "ASPNETCORE_ENVIRONMENT",
    "ESZOP_AZURE_EVENTBUS_CONN_STR",
    "ESZOP_SQLSERVER_CONN_STR",
    "ESZOP_REDIS_CONN_STR",
    "ESZOP_AZURE_STORAGE_CONN_STR"
)

Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
    $service_dir = Resolve-ServiceLocation -ServiceName $service
    $scripts_dir = Join-Path (Resolve-Path $service_dir) "scripts" "docker"
    $start_script = Join-Path $scripts_dir "start-service.ps1"

    if(Test-Path $start_script) {
        Write-Host "Run $start_script"
        & $start_script -ImageTag $ImageTag
    }
}