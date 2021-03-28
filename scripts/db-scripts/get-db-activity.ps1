param (
    [Parameter(Mandatory = $true)]
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

$server_name = "eszop-$environment_prefix-$Service-sqlserver"

$activities = (Get-AzSqlDatabaseActivity `
        -ServerName $server_name `
        -DatabaseName "eszop" `
        -ResourceGroupName "eszop-$environment_prefix") | `
    Sort-Object -Descending StartTime

if(-not($Wide)) {
    $activities = $activities | Select-Object -Property Operation, OperationId, State, PercentComplete, StartTime, EndTime -First $Num
}

$activities