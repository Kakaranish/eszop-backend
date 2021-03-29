param(
    [Parameter(Mandatory = $true)]
    [string] $DbUsername,

    [Parameter(Mandatory = $true)]
    [string] $DbPassword,

    [switch] $AutoApprove
)

Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Resolve-ServiceLocation.psm1 -Force

if (-not($ConnectionString)) {
    $ConnectionString = $default_connection_string
}

$services = @("carts", "identity", "notification", "offers", "orders")

$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "DevelopmentLocal"
}

$environment_prefix = Resolve-EnvPrefix -Environment $environment
$appsettings_filename = "appsettings.$environment.json"

$connection_string_template = "Server=tcp:eszop-{env_prefix}-sqlserver.database.windows.net,1433;Initial Catalog=eszop-{env_prefix}-{service_name}-db;Persist Security Info=False;User ID={db_username};Password={db_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

# ------------------------------------------------------------------------------

if (-not($AutoApprove.IsPresent)) {
    Write-Host "Those files will be changed:"
    foreach ($service in $services) {
        $service_location = Resolve-ServiceLocation -ServiceName $service
        Write-Host (Resolve-Path -Path (Join-Path $service_location $appsettings_filename))
    }
    $choice = Read-Host "Do you want to continue (y/n)"
    if ($choice -ne "y") {
        exit
    }
}

foreach ($service in $services) {
    $service_location = Resolve-ServiceLocation -ServiceName $service
    $appsettings_path = Resolve-Path -Path (Join-Path $service_location $appsettings_filename)
    if (-not(Test-Path -Path $appsettings_path)) {
        Write-Warning "[SKIP] File $appsettings_path does not exist"
        continue
    }

    $connection_string = $connection_string_template `
        -replace "{env_prefix}", $environment_prefix `
        -replace "{service_name}", $service `
        -replace "{db_username}", $DbUsername `
        -replace "{db_password}", $DbPassword

    $appsettings_json = Get-Content -Path $appsettings_path | ConvertFrom-Json
    $appsettings_json.ConnectionStrings | Add-Member -Name "SqlServer" -MemberType NoteProperty $connection_string -Force
    $appsettings_json | ConvertTo-Json -Depth 9 | Set-Content $appsettings_path
}

Write-Host "appsettings.json files updated successfully"