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

$topic_name = "eszop-$cloud_env_prefix-event-bus-topic"
$sub_name_template = "eszop-$cloud_env_prefix-event-bus-{service_name}-sub"
$container_name = "eszop-$cloud_env_prefix-storage-container"
$appsettings_filename = "appsettings.$env_name.json"

$services = @("carts", "identity", "notification", "offers", "orders")

if (-not($AutoApprove.IsPresent)) {
    Write-Host "Those files will be changed:"
    foreach ($service in $services) {
        $service_location = Resolve-ServiceLocation -ServiceName $service
        Write-Host (Resolve-Path -Path (Join-Path $service_location "Properties" "launchSettings.json"))
        Write-Host (Resolve-Path -Path (Join-Path $service_location $appsettings_filename))
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

    # ---  EDIT launchSettings.json  -------------------------------------------

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

    # ---  EDIT appsettings.json  ----------------------------------------------

    $appsettings_path = Resolve-Path -Path (Join-Path $service_location $appsettings_filename)
    if (-not(Test-Path -Path $appsettings_path)) {
        Write-Warning "[SKIP] File $appsettings_path does not exist"
        continue
    }

    $sub_name = $sub_name_template -replace "{service_name}", $service
    $appsettings_json = Get-Content -Path $appsettings_path | ConvertFrom-Json

    $appsettings_json.EventBus.AzureEventBus | Add-Member -Name "TopicName" -MemberType NoteProperty -Value $topic_name -Force
    $appsettings_json.EventBus.AzureEventBus | Add-Member -Name "SubscriptionName" -MemberType NoteProperty -Value $sub_name -Force
    
    if($service -eq "offers") {
        $appsettings_json.AzureStorage | Add-Member -Name "ContainerName" -MemberType NoteProperty -Value $container_name -Force
    }

    $appsettings_json | ConvertTo-Json -Depth 9 | Set-Content $appsettings_path
}
