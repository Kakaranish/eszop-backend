# ------------------------------------------------------------------------------
#
# This script publishes packages created with 'Build-Packages.ps1' to
# GoogleCloudStorage bucket. Name of bucket is specified in config/global.yaml
#
# ------------------------------------------------------------------------------

param (
  [Parameter(Mandatory = $true)]
  [string] $BuildSuffix,

  [Parameter(Mandatory = $true)]
  [string] $BuildDirectory
)

$scripts_dir = "$PSScriptRoot\..\.."
Import-Module "${scripts_dir}\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

if (-not(Test-Path $BuildDirectory)) {
  Write-Error "There is no such directory" -ErrorAction Stop
}

$global_config = Get-GlobalConfig

$services = @("gateway", "offers", "identity", "carts", "orders", "notification")

foreach ($service in $services) {
  $service_build_filename = "$service`_$BuildSuffix.zip"
  $service_build_path = Join-Path $BuildDirectory $service_build_filename
  if (-not(Test-Path $service_build_path)) {
    Write-Warning "$service_build_filename cannot be published because does not exist"
    continue
  }

  Write-Host "[INFO] Publishing ${service_build_filename} in gcs bucket $($global_config.GCP_PACKAGES_STORAGE)" -ForegroundColor DarkGreen
  New-GcsObject -Bucket $global_config.GCP_PACKAGES_STORAGE -File $service_build_path | Out-Null
}