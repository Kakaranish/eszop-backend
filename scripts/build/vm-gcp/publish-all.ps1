# ------------------------------------------------------------------------------
#
# This script publishes packages created with build-all.ps1
# GoogleCloudStorage bucket name for storing packages is specified in scripts/GcpConfig.psm1
#
# ------------------------------------------------------------------------------

param (
  [Parameter(Mandatory = $true)]
  [string] $BuildSuffix,

  [Parameter(Mandatory = $true)]
  [string] $BuildDirectory
)

Import-Module "$PSScriptRoot\..\..\GcpConfig.psm1" -Force

if (-not(Test-Path $BuildDirectory)) {
  Write-Error "There is no such directory" -ErrorAction Stop
}

$services = @("gateway", "offers", "identity", "carts", "orders", "notification")

foreach ($service in $services) {
  $service_build_filename = "$service`_$BuildSuffix.zip"
  $service_build_path = Join-Path $BuildDirectory $service_build_filename
  if (-not(Test-Path $service_build_path)) {
    Write-Warning "$service_build_filename cannot be published because does not exist"
    continue
  }

  Write-Host "[INFO] Publishing $service_build_filename in gcs bucket $GCP_PACKAGES_STORAGE" -ForegroundColor DarkGreen
  New-GcsObject -Bucket $GCP_PACKAGES_STORAGE -File $service_build_path | Out-Null
}