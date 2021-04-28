# ------------------------------------------------------------------------------
#
# This script builds solution and creates .zip packages with services' binaries.
# An output of the script is build suffix for created packages.
# The build suffix can be used to publish packages to GCS with 
# 'Publish-Packages.ps1' script.
#
# Sample script call:
# $ .\Build-Packages.ps1 -OutputDirectory ..\..\..\build
#
# ------------------------------------------------------------------------------

param (
  [Parameter(Mandatory = $true)]
  [string] $OutputDirectory
)

Import-Module "$PSScriptRoot\..\..\modules\Resolve-ServiceLocation.psm1" -Force

# ------------------------------------------------------------------------------

if (-not(Test-Path $OutputDirectory)) {
  Write-Error "There is no such directory" -ErrorAction Stop
}

$services = @("gateway", "offers", "identity", "carts", "orders", "notification")
$build_suffix = Get-Date -UFormat "+%Y%m%d_%H%M%S"
$build_dir = Resolve-Path $OutputDirectory

foreach ($service in $services) {
  $service_path = Resolve-ServiceLocation -ServiceName $service
  $publish_path = (Join-Path $build_dir $service)
  if (-not(Test-Path -Path $publish_path)) {
    New-Item -ItemType Directory -Path $publish_path | Out-Null
  }

  Write-Host "[INFO] Build $service_path" -ForegroundColor DarkGreen
  dotnet publish -o $publish_path -c Release $service_path

  $zip_filename = "${service}`_${build_suffix}.zip"
  Write-Host $zip_filename
  Compress-Archive -CompressionLevel Optimal -Path $publish_path\* -DestinationPath $build_dir\$zip_filename
  Remove-Item -Path $publish_path -Recurse -Force | Out-Null
}

Write-Host "[INFO] Build $build_suffix succeeded" -ForegroundColor Green

$resolved_build_dir = Resolve-Path $OutputDirectory
$build_info = @{ 
  "BuildSuffix"    = $build_suffix;
  "BuildDirectory" = $resolved_build_dir.Path;
}

$cache_path = "$PSScriptRoot\output\cache.yaml"
if (-not(Test-Path "$PSScriptRoot\output")) {
  New-Item -ItemType Directory -Name "output" | Out-Null
}
New-Item -ItemType File -Path $cache_path -Force | Out-Null
$build_info | ConvertTo-Yaml | Set-Content $cache_path -NoNewline