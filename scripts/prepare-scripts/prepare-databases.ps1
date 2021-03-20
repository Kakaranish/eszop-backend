[CmdletBinding()]
param (
    [string] $ConfigEnv = "DevelopmentLocal"
)

$previous_env = $env:ASPNETCORE_ENVIRONMENT

$env:ASPNETCORE_ENVIRONMENT=$ConfigEnv
$env:ESZOP_DB_SEED=$true
$lookup_dirs = @(
    "$PSScriptRoot\..\..\Carts.API",
    "$PSScriptRoot\..\..\Identity.API",
    "$PSScriptRoot\..\..\NotificationService",
    "$PSScriptRoot\..\..\Offers.API",
    "$PSScriptRoot\..\..\Orders.API" 
);

foreach ($dir in $lookup_dirs) {
    $db_update_command = "dotnet ef database update --project $dir"
    Write-Host "RUN: $db_update_command"
    Invoke-Expression $db_update_command
}

$env:ESZOP_DB_SEED=$false
$env:ASPNETCORE_ENVIRONMENT=$previous_env