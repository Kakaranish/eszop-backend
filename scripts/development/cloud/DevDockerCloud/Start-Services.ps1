param(
  [string] $ImageTag = "latest",

  [Parameter(Mandatory = $true)]
  [ValidateSet("dev", "staging", "prod")] 
  [string] $CloudEnv
)

$scripts_dir = "$PSScriptRoot\..\..\.."
Import-Module "${scripts_dir}\modules\Resolve-ServiceLocation.psm1" -Force
Import-Module "${scripts_dir}\modules\Get-MultipleEnvVariables.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\modules\Set-MultipleEnvVariables.psm1" -Force -DisableNameChecking
Import-Module "${scripts_dir}\modules\Set-AppEnvVariables.psm1" -Force -DisableNameChecking

# ------------------------------------------------------------------------------

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
  "ASPNETCORE_ENVIRONMENT"
)
$env_vars_backup = Get-MultipleEnvVariables -Variables $env_vars_to_backup

$env:ASPNETCORE_ENVIRONMENT = "DevDockerCloud"
Set-AppEnvVariables -CloudEnv $CloudEnv

Write-Host $env:ESZOP_CLIENT_URI

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")
foreach ($service in $services) {
  $service_dir = Resolve-ServiceLocation -ServiceName $service
  $scripts_dir = Join-Path (Resolve-Path $service_dir) ".." "scripts" "docker" "DevDockerCloud"
  $start_script = Join-Path $scripts_dir "start-service.ps1"

  if (Test-Path $start_script) {
    Write-Host "Run $start_script"
    . $start_script -ImageTag $ImageTag -TargetCloudEnvPrefix $CloudEnv
  }
}

Set-MultipleEnvVariables -EnvDictionary $env_vars_backup