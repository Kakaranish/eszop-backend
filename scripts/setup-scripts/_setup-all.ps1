
#---   CONIFG   ----------------------------------------------------------------

$db_username = ""
$db_password = ""
$redis_conn_str = ""
$storage_conn_str = ""
$event_bus_conn_str = ""

#-------------------------------------------------------------------------------

& $PSScriptRoot\set-environment.ps1

Write-Host "[RUN] Set Storage"
& "$PSScriptRoot\set-storage.ps1" -ConnectionString "$storage_conn_str" -AutoApprove

Write-Host "[RUN] Set EventBus"
& "$PSScriptRoot\set-eventbus.ps1" -ConnectionString "$event_bus_conn_str" -UseAzureEventBus -AutoApprove

Write-Host "[RUN] Set Redis Connection String"
& "$PSScriptRoot\set-redis-connection-string.ps1" -ConnectionString "$redis_conn_str" -AutoApprove

Write-Host "[RUN] Set SqlServer Connection Strings"
& "$PSScriptRoot\set-sqlserver-connection-strings.ps1" -DbUsername $db_username -DbPassword $db_password -AutoApprove

Write-Host "[RUN] Apply db migrations"
& "$PSScriptRoot\apply-db-migrations.ps1"