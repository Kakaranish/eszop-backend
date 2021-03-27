Import-Module $PSScriptRoot\..\modules\Resolve-ServiceLocation.psm1 -Force

$default_environment = "Development"
$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = $default_environment
}

# To avoid RabbitMq start
$env:ESZOP_DB_SEED = $true

$services = @("carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
    $service_location = Resolve-ServiceLocation -ServiceName $service
    $db_update_command = "dotnet ef database update --project $service_location"
    Write-Host "RUN: $db_update_command"
    Invoke-Expression $db_update_command
}

$env:ESZOP_DB_SEED = $false