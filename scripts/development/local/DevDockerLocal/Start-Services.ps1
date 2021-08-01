param(
  [string] $ImageTag = "latest"
)

$modules_dir = "$PSScriptRoot\..\..\..\modules"
Import-Module "${modules_dir}\Resolve-ServiceLocation.psm1" -Force

# ------------------------------------------------------------------------------

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
  $service_dir = Resolve-ServiceLocation -ServiceName $service
  $scripts_dir = Join-Path (Resolve-Path $service_dir) ".." "scripts" "docker" "DevDockerLocal"
  $start_script = Join-Path $scripts_dir "start-service.ps1"

  if (Test-Path $start_script) {
    Write-Host "Run $start_script"
    & $start_script -ImageTag $ImageTag
  }
}