param(
  [Parameter(Mandatory = $true)]
  [string] $MigrationName
)

$infra_dir = "Orders.Infrastructure"
$app_dir = "Orders.API"

$prev_env = $env:ASPNETCORE_ENVIRONMENT
$env:ASPNETCORE_ENVIRONMENT = "DevHostLocal"
$env:ESZOP_DB_SEED = "true"

dotnet ef --startup-project "$PSScriptRoot\..\${app_dir}" --project "$PSScriptRoot\..\${infra_dir}" migrations add $MigrationName

$env:ASPNETCORE_ENVIRONMENT = $prev_env
$env:ESZOP_DB_SEED = "false"