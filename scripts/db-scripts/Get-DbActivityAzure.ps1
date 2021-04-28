param (
  [Parameter(Mandatory = $true)]
  [ValidateSet("offers", "identity", "carts", "orders", "notification")] 
  [string] $Service,

  [Parameter(Mandatory = $true)]
  [ValidateSet("dev", "staging", "prod")]
  [string] $CloudEnv,

  [int] $Num = 5,

  [switch] $Wide
)

Import-Module "$PSScriptRoot\..\modules\Get-GlobalConfig.psm1" -Force

# ------------------------------------------------------------------------------

$server_name = "eszop-$CloudEnv-sqlserver"
$db_name = "eszop-$CloudEnv-$Service-db"

$activities = (Get-AzSqlDatabaseActivity `
    -ServerName $server_name `
    -DatabaseName $db_name `
    -ResourceGroupName "eszop-$CloudEnv") | `
  Sort-Object -Descending StartTime

if (-not($Wide)) {
  $activities = $activities | Select-Object -Property Operation, OperationId, State, PercentComplete, StartTime, EndTime -First $Num
}

$activities