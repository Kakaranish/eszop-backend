param(
    [string] $ConnectionString,
    [switch] $UseAzureEventBus
)

# ---  CONFIGURATION  ----------------------------------------------------------
$default_connection_string = "ANYTHING"

# ------------------------------------------------------------------------------

if(-not($ConnectionString)){
    $ConnectionString = $default_connection_string
}

$lookup_dirs = @(
    "$PSScriptRoot\..\Carts.API",
    "$PSScriptRoot\..\Identity.API",
    "$PSScriptRoot\..\NotificationService",
    "$PSScriptRoot\..\Offers.API",
    "$PSScriptRoot\..\Orders.API" 
);

$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "DevelopmentLocal"
}

$appsettings_filename = "appsettings.$environment.json"
$appsettings_files = $lookup_dirs | `
    ForEach-Object { Join-Path (Resolve-Path $_) $appsettings_filename }

Write-Host "Those files will be changed:"
Write-Host ($appsettings_files -join "`n")
$choice = Read-Host "Do you want to continue (y/n)"
if ($choice -ne "y") {
    exit
}

foreach ($appsettings_file in $appsettings_files) {
    if (-not(Test-Path $appsettings_file)) {
        Write-Warning "[SKIP] File $appsettings_file does not exist"
        continue
    }

    $app_settings_json = Get-Content -Path $appsettings_file | ConvertFrom-Json

    $app_settings_json.EventBus.AzureEventBus | Add-Member -Name "ConnectionString" -MemberType NoteProperty -Value $ConnectionString -Force
    $app_settings_json.EventBus | Add-Member -Name "UseAzureEventBus" -MemberType NoteProperty -Value $UseAzureEventBus.IsPresent -Force
    
    $app_settings_json | ConvertTo-Json -Depth 9 | Set-Content $appsettings_file
}

Write-Host "appsettings.json files updated successfully"