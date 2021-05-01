# ------------------------------------------------------------------------------
#
# This script starts dependent services for eszop applications.
# Those services are:
# - SQL Server DBs - for apps: Offers, Identity, Carts, Orders, Notification
# - RedisDb - for apps: Identity
# - RabbitMq - for apps: Offers, Identity, Carts, Orders, Notification
# 
# NOTE:
# When first run, use '-ApplyMigrations' flag to apply migrations to newly created
# databases. Without this applications will throw exceptions.
#
# ------------------------------------------------------------------------------

param (
  [switch] $ApplyMigrations
)

$scripts_dir = "$PSScriptRoot\..\..\.."

# ------------------------------------------------------------------------------

Write-Host "[INFO] Starting services" -ForegroundColor Green

Get-ChildItem -Recurse -Path $PSScriptRoot | `
  Where-Object {
  $_.Name -eq "start-dependent-services.ps1" -and $_.DirectoryName -ne $PSScriptRoot 
} | `
  ForEach-Object {
  & $_
}

if ($ApplyMigrations.IsPresent) {
  Write-Host "[INFO] Applying migrations to the local databases" -ForegroundColor Green
  & "${scripts_dir}\db-scripts\Apply-DbMigrations.ps1" -Local
}