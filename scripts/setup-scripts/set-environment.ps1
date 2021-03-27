$environments = @(
    "DevelopmentLocal",
    "Development",
    "Staging"
)
Write-Host "Choose environment:"
For ($i=0; $i -lt $environments.Count; $i++)  {
  Write-Host "$($i+1): $($environments[$i])"
}

[int]$choice = Read-Host "Press the number to select an environment"

if($choice -lt 1 -or $choice -gt $environments.Count) {
    Write-Host "Invalid choice"
    exit
}

$environment = $environments[$choice-1]
$env:ASPNETCORE_ENVIRONMENT = $environment
Write-Host "Set environment to $environment."