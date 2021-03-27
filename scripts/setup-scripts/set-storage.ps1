param(
    [Parameter(Mandatory = $true)]
    [string] $ConnectionString,

    [switch] $AutoApprove
)

Import-Module $PSScriptRoot\..\modules\Resolve-ServiceLocation.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force

$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "DevelopmentLocal"
}

$service = "offers"
$environment_prefix = Resolve-EnvPrefix -Environment $environment

$appsettings_filename = "appsettings.$environment.json"
$service_location = Resolve-ServiceLocation -ServiceName $service
$appsettings_path = Resolve-Path -Path (Join-Path $service_location $appsettings_filename)

#-------------------------------------------------------------------------------

if (-not($AutoApprove.IsPresent)) {
    Write-Host "This file will be changed:"
    Write-Host $appsettings_path
    $choice = Read-Host "Do you want to continue (y/n)"
    if ($choice -ne "y") {
        exit
    }
}

if (-not(Test-Path -Path $appsettings_path)) {
    Write-Warning "[SKIP] File $appsettings_path does not exist"
    continue
}

$storage_section = @"
{
    "ConnectionString": "$ConnectionString",
    "ContainerName": "eszop-$environment_prefix-storage-container"
}
"@

$appsettings_json = Get-Content -Path $appsettings_path | ConvertFrom-Json
$appsettings_json | Add-Member -Name "AzureStorage" -MemberType NoteProperty -Value (ConvertFrom-Json $storage_section) -Force
$appsettings_json | ConvertTo-Json -Depth 9 | Set-Content $appsettings_path

Write-Host "$appsettings_path updated successfully"