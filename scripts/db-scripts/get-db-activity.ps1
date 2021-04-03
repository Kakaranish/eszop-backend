param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("offers", "identity", "carts", "orders", "notification")] 
    [string] $Service,

    [int] $Num = 5,

    [switch] $Wide
)

Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Require-EnvironmentVariables.psm1 -Force -DisableNameChecking

$required_env_variables = @( "ASPNETCORE_ENVIRONMENT" )
Require-EnvironmentVariables -EnvironmentVariables $required_env_variables

$environment_prefix = Resolve-EnvPrefix -Environment $env:ASPNETCORE_ENVIRONMENT
$server_name = "eszop-$environment_prefix-sqlserver"
$db_name = "eszop-$environment_prefix-$Service-db"

$activities = (Get-AzSqlDatabaseActivity `
        -ServerName $server_name `
        -DatabaseName $db_name `
        -ResourceGroupName "eszop-$environment_prefix") | `
    Sort-Object -Descending StartTime

if(-not($Wide)) {
    $activities = $activities | Select-Object -Property Operation, OperationId, State, PercentComplete, StartTime, EndTime -First $Num
}

$activities