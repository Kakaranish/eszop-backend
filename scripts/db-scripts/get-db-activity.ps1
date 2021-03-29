param (
    [Parameter(Mandatory = $true)]
    [ValidateSet("offers", "identity", "carts", "orders", "notification")] 
    [string] $Service,

    [int] $Num = 5,

    [switch] $Wide
)

Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force

$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "DevelopmentLocal"
}
$environment_prefix = Resolve-EnvPrefix -Environment $environment
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