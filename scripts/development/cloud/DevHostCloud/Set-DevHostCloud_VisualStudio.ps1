# ------------------------------------------------------------------------------
# 
# This script should be executed before running application on DevHostCloud environment
#
# It prepares environment variables in "launchSettings.json" files and
# replaces some values in "appsettings.json" files for each service
#
# DB & EventBus credentials and connection strings are taken
# from config/environments/${env}.yaml
#
# ------------------------------------------------------------------------------

param(
  [Parameter(Mandatory = $true)]
  [ValidateSet("dev", "staging", "prod")]
  [string] $CloudEnv,

  [switch] $AutoApprove
)

$scripts_dir = "$PSScriptRoot\..\..\.."

Import-Module "${scripts_dir}\modules\Resolve-ServiceLocation.psm1" -Force
Import-Module "${scripts_dir}\modules\Prepare-SqlConnectionString.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\modules\Get-AppsConfig.psm1" -Force

# ------------------------------------------------------------------------------

$env_name = "DevHostCloud"
$apps_config = Get-AppsConfig -CloudEnv $CloudEnv

$container_name = "eszop-${CloudEnv}-storage-container"
$appsettings_filename = "appsettings.${env_name}.json"

$services = @("carts", "identity", "notification", "offers", "orders")

if (-not($AutoApprove.IsPresent)) {
  Write-Host "Those files will be changed:"
  foreach ($service in $services) {
    $service_location = Resolve-ServiceLocation -ServiceName $service
    Write-Host (Resolve-Path -Path (Join-Path $service_location "Properties" "launchSettings.json"))
    Write-Host (Resolve-Path -Path (Join-Path $service_location $appsettings_filename))
  }
  $choice = Read-Host "Do you want to continue (y/n)"
  if ($choice -ne "y") { exit }
}

$redis_conn_str = "$($apps_config.REDIS_ADDRESS):$($apps_config.REDIS_PORT),password=$($apps_config.REDIS_PASSWORD)"

foreach ($service in $services) {
  $service_location = Resolve-ServiceLocation -ServiceName $service

  $launch_settings_path = Resolve-Path -Path (Join-Path $service_location "Properties" "launchSettings.json")
  if (-not(Test-Path -Path $launch_settings_path)) {
    Write-Warning "[SKIP] File $launch_settings_path does not exist"
    continue
  }
    
  Write-Host "[Setting up $service]"

  $env_variables_to_set = @{
    "ESZOP_AZURE_STORAGE_CONN_STR"    = $apps_config.ESZOP_AZURE_STORAGE_CONN_STR;
    "ESZOP_AZURE_EVENTBUS_CONN_STR"   = $apps_config.ESZOP_AZURE_EVENTBUS_CONN_STR;
    "ESZOP_REDIS_CONN_STR"            = $redis_conn_str;
    "ESZOP_SQLSERVER_CONN_STR"        = (Prepare-SqlConnectionString `
        -EnvironmentPrefix $CloudEnv `
        -ServiceName $service `
        -DbUsername $apps_config.SQLSERVER_USERNAME `
        -DbPassword $apps_config.SQLSERVER_PASSWORD);
    "ESZOP_AZURE_EVENTBUS_TOPIC_NAME" = "eszop-${CloudEnv}-event-bus-topic";
    "ESZOP_AZURE_EVENTBUS_SUB_NAME"   = "eszop-${CloudEnv}-event-bus-${service}-sub"
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

  if ($service -eq "offers") {
    $appsettings_path = Resolve-Path -Path (Join-Path $service_location $appsettings_filename)
    if (-not(Test-Path -Path $appsettings_path)) {
      Write-Warning "[SKIP] File $appsettings_path does not exist"
      continue
    }
        
    $appsettings_json = Get-Content -Path $appsettings_path | ConvertFrom-Json
    $appsettings_json.AzureStorage | Add-Member -Name "ContainerName" -MemberType NoteProperty -Value $container_name -Force
    $appsettings_json | ConvertTo-Json -Depth 9 | Set-Content $appsettings_path
  }
}