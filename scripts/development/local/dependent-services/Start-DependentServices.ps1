param (
  [switch] $ApplyMigrations
)

Write-Host "[INFO] Starting services" -ForegroundColor Green

Get-ChildItem -Recurse | `
  Where-Object {
  $_.Name -eq "start-dependent-services.ps1" -and $_.DirectoryName -ne $PSScriptRoot 
} | `
  ForEach-Object {
  & $_
}

# TODO:
# if ($ApplyMigrations.IsPresent) {
#   Write-Host "[INFO] Applying migrations to the databases" -ForegroundColor Green

#   & "$PSScriptRoot\apply-db-migrations.ps1"
# }