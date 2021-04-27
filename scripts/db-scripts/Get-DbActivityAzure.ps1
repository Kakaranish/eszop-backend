param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("offers", "identity", "carts", "orders", "notification")] 
    [string] $Service,

    [int] $Num = 5,

    [switch] $Wide
)

Import-Module "$PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1" -Force
Import-Module "$PSScriptRoot\..\modules\Require-CloudEnvVar.psm1" -Force -DisableNameChecking -Scope Local
Import-Module "$PSScriptRoot\..\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$cloud_env = Require-CloudEnvVar
$server_name = "eszop-${cloud_env}-sqlserver"
$db_name = "eszop-${cloud_env}-$Service-db"

$activities = (Get-AzSqlDatabaseActivity `
        -ServerName $server_name `
        -DatabaseName $db_name `
        -ResourceGroupName "eszop-${cloud_env}") | `
    Sort-Object -Descending StartTime

if (-not($Wide)) {
    $activities = $activities | Select-Object -Property Operation, OperationId, State, PercentComplete, StartTime, EndTime -First $Num
}

$activities