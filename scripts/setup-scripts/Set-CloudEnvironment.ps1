Import-Module "$PSScriptRoot\..\modules\Make-Choice.psm1" -Force -DisableNameChecking

$environments = @("dev", "staging", "prod")
$choice = Make-Choice `
    -Title "Choose environment" `
    -Choices $environments

$env:ESZOP_CLOUD_ENV = $choice
    
Write-Host "Set cloud environment to $choice"