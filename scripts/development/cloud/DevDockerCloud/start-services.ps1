param(
    [string] $ImageTag = "latest",

    [Parameter(Mandatory = $true)]    
    [ValidateSet("DevCloud", "Staging")]
    [string] $TargetCloudEnv = "Staging",

    [switch] $SetUpEnvironmentVariables
)
$modules_dir = "$PSScriptRoot\..\..\..\modules"
Import-Module "${modules_dir}\Resolve-ServiceLocation.psm1" -Force
Import-Module "${modules_dir}\Require-EnvironmentVariables.psm1" -Force -DisableNameChecking
Import-Module "${modules_dir}\Resolve-EnvPrefix.psm1" -Force -DisableNameChecking

if($SetUpEnvironmentVariables.IsPresent) {
    & "$PSScriptRoot\set-up-DevDockerCloud-env.ps1"
}

$required_env_variables = @(
    "ASPNETCORE_ENVIRONMENT",
    "ESZOP_AZURE_EVENTBUS_CONN_STR",
    "ESZOP_REDIS_CONN_STR",
    "ESZOP_AZURE_STORAGE_CONN_STR",
    "ESZOP_SQLSERVER_CONN_STR_OFFERS",
    "ESZOP_SQLSERVER_CONN_STR_IDENTITY",
    "ESZOP_SQLSERVER_CONN_STR_CARTS",
    "ESZOP_SQLSERVER_CONN_STR_ORDERS",
    "ESZOP_SQLSERVER_CONN_STR_NOTIFICATION"
)

$target_cloud_env_prefix = Resolve-EnvPrefix -Environment $TargetCloudEnv

Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$services = @("gateway", "carts", "identity", "notification", "offers", "orders")

foreach ($service in $services) {
    $service_dir = Resolve-ServiceLocation -ServiceName $service
    $scripts_dir = Join-Path (Resolve-Path $service_dir) "scripts" "docker" "DevDockerCloud"
    $start_script = Join-Path $scripts_dir "start-service.ps1"

    if(Test-Path $start_script) {
        Write-Host "Run $start_script"
        & $start_script -ImageTag $ImageTag -TargetCloudEnvPrefix $target_cloud_env_prefix
    }
}