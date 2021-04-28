# ------------------------------------------------------------------------------
#
# This script stops dependent services which were started with
# 'Start-DependentServices.ps1' script.
#
# ------------------------------------------------------------------------------

Get-ChildItem -Recurse | `
  Where-Object {
  $_.Name -eq "stop-dependent-services.ps1" -and $_.DirectoryName -ne $PSScriptRoot
} | `
  ForEach-Object {
  & $_
}