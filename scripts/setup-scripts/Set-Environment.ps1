Import-Module "$PSScriptRoot\..\modules\Make-Choice.psm1" -Force -DisableNameChecking

$environments = @(
  "DevHostLocal",
  "DevHostCloud",
  "DevDockerLocal",
  "DevDockerCloud",
  "Staging"
)

$choice = Make-Choice `
  -Title "Choose environment" `
  -Choices $environments

$env:ASPNETCORE_ENVIRONMENT = $choice
    
Write-Host "Set environment to $choice"