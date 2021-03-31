param(
    [switch] $AutoApprove
)

Import-Module $PSScriptRoot\..\modules\Resolve-ServiceLocation.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Prepare-SqlConnectionString.psm1 -Force -DisableNameChecking

# ---  CONFIG  -----------------------------------------------------------------

$ESZOP_AZURE_STORAGE_CONN_STR = ""
$ESZOP_AZURE_EVENTBUS_CONN_STR = ""
$ESZOP_REDIS_CONN_STR = ""

$db_username = ""
$db_password = ""

$cloud_env_prefix = "staging"

# ------------------------------------------------------------------------------

$services = @("carts", "identity", "notification", "offers", "orders")

if (-not($AutoApprove.IsPresent)) {
    Write-Host "Those files will be changed:"
    foreach ($service in $services) {
        $service_location = Resolve-ServiceLocation -ServiceName $service
        Write-Host (Resolve-Path -Path (Join-Path $service_location "Properties" "launchSettings.json"))
    }
    $choice = Read-Host "Do you want to continue (y/n)"
    if ($choice -ne "y") {
        exit
    }
}

foreach ($service in $services) {
    $service_location = Resolve-ServiceLocation -ServiceName $service
    $launch_settings_path = Resolve-Path -Path (Join-Path $service_location "Properties" "launchSettings.json")
    if (-not(Test-Path -Path $launch_settings_path)) {
        Write-Warning "[SKIP] File $launch_settings_path does not exist"
        continue
    }
    
    Write-Host "[Setting up $service]"

    $env_variables_to_set = @{
        "ESZOP_AZURE_STORAGE_CONN_STR"  = $ESZOP_AZURE_STORAGE_CONN_STR;
        "ESZOP_AZURE_EVENTBUS_CONN_STR" = $ESZOP_AZURE_EVENTBUS_CONN_STR;
        "ESZOP_REDIS_CONN_STR"          = $ESZOP_REDIS_CONN_STR;
        "ESZOP_SQLSERVER_CONN_STR"      = Prepare-SqlConnectionString `
            -EnvironmentPrefix $cloud_env_prefix `
            -ServiceName $service `
            -DbUsername $db_username `
            -DbPassword $db_password;
    }

    $launch_settings_json = Get-Content -Path $launch_settings_path | ConvertFrom-Json -Depth 9
    
    $cloud_profile_name = ($launch_settings_json.profiles.PSObject.Properties | Where-Object { $_.Name -match "Cloud" })[0].Name
    $profile_env_vars = $launch_settings_json.profiles.$cloud_profile_name.environmentVariables

    foreach ($env_variable_to_set in $env_variables_to_set.Keys) {
        if (($profile_env_vars.PSObject.Properties | Where-Object { $_.Name -eq $env_variable_to_set }).Count) {
            $val_to_set = $env_variables_to_set.$env_variable_to_set
            $launch_settings_json.profiles.$cloud_profile_name.environmentVariables | `
                Add-Member -Name $env_variable_to_set -MemberType NoteProperty -Value $val_to_set -Force
            
            Write-Host "Updated $env_variable_to_set"
        }
    }
    $launch_settings_json | ConvertTo-Json -Depth 9 | Set-Content $launch_settings_path
}
