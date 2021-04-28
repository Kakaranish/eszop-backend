param (
  [switch] $ApplyMigrations
)

$scripts_dir = "$PSScriptRoot\..\..\.."

# ------------------------------------------------------------------------------

Write-Host "[INFO] Starting services" -ForegroundColor Green

Get-ChildItem -Recurse | `
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