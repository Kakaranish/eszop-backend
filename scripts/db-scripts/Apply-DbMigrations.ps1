Import-Module "$PSScriptRoot\..\modules\Resolve-ServiceLocation.psm1" -Force
Import-Module "$PSScriptRoot\..\modules\Make-Choice.psm1" -Force -DisableNameChecking
Import-Module "$PSScriptRoot\..\modules\Get-MultipleEnvVariables.psm1" -Force -DisableNameChecking
Import-Module "$PSScriptRoot\..\modules\Set-MultipleEnvVariables.psm1" -Force -DisableNameChecking

# ------------------------------------------------------------------------------

$env_type_choices = @("local", "cloud")
$env_type = Make-Choice `
  -Title "Choose environment type" `
  -Choices $env_type_choices

$env_vars_to_backup = @(
  "ESZOP_CLIENT_URI",
  "ESZOP_AZURE_EVENTBUS_CONN_STR",
  "ESZOP_AZURE_STORAGE_CONN_STR",
  "ESZOP_REDIS_CONN_STR",
  "ESZOP_SQLSERVER_CONN_STR_OFFERS",
  "ESZOP_SQLSERVER_CONN_STR_IDENTITY",
  "ESZOP_SQLSERVER_CONN_STR_CARTS",
  "ESZOP_SQLSERVER_CONN_STR_ORDERS",
  "ESZOP_SQLSERVER_CONN_STR_NOTIFICATION",
  "ESZOP_SQLSERVER_CONN_STR",
  "ASPNETCORE_ENVIRONMENT",
  "ESZOP_DB_SEED"
)
$env_vars_backup = Get-MultipleEnvVariables -Variables $env_vars_to_backup

$env:ESZOP_DB_SEED = $true

if ($env_type -eq "local") {
  $env:ASPNETCORE_ENVIRONMENT = "DevHostLocal"
  $env:ESZOP_AZURE_STORAGE_CONN_STR = "ANYTHING"
}
else {
  $target_cloud_envs = [ordered] @{
    "dev"     = "Staging";
    "staging" = "Staging";
    "prod"    = "Production";
  }
  $target_cloud_env = Make-Choice `
    -Title "Choose target cloud environment" `
    -Choices $target_cloud_envs.Keys
        
  $env:ASPNETCORE_ENVIRONMENT = $target_cloud_envs[$target_cloud_env]

  . "$PSScriptRoot\..\setup-scripts\Set-AppEnvVariables.ps1" -CloudEnv $target_cloud_env
}
    
$services = @("carts", "identity", "notification", "offers", "orders")
foreach ($service in $services) {
  if ($env_type -eq "cloud") {
    $var_name = "${sql_conn_str_name_prefix}$($service.ToUpperInvariant())"
    $service_sql_conn_str = Invoke-Expression "`$env:${var_name}"
    $env:ESZOP_SQLSERVER_CONN_STR = $service_sql_conn_str
  }

  $service_location = Resolve-ServiceLocation -ServiceName $service
  $db_update_command = "dotnet ef database update --project $service_location"
  Write-Host "RUN: $db_update_command"
  Invoke-Expression $db_update_command
}

Set-MultipleEnvVariables -EnvDictionary $env_vars_backup
Write-Host "[INFO] Enviroment variables rolled back" -ForegroundColor Green