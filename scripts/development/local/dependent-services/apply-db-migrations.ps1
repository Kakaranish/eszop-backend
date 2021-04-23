# ------------------------------------------------------------------------------
#
# This script applies migrations to local databases
#
# ------------------------------------------------------------------------------

$scripts_dir = "$PSScriptRoot\..\..\.."
Import-Module "${scripts_dir}\modules\Resolve-ServiceLocation.psm1" -Force

# To avoid RabbitMq start
$env:ASPNETCORE_ENVIRONMENT = "DevHostLocal"
$env:ESZOP_DB_SEED = $true

$services = @("carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
    $service_location = Resolve-ServiceLocation -ServiceName $service
    $db_update_command = "dotnet ef database update --project $service_location"
    Write-Host "RUN: $db_update_command"
    Invoke-Expression $db_update_command
}

$env:ESZOP_DB_SEED = $false