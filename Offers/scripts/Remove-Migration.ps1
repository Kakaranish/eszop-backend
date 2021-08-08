$infra_dir = "Offers.Infrastructure"
$app_dir = "Offers.API"

$prev_env = $env:ASPNETCORE_ENVIRONMENT
$env:ASPNETCORE_ENVIRONMENT = "DevHostLocal"
$env:ESZOP_DB_SEED = "true"

dotnet ef --startup-project "$PSScriptRoot\..\${app_dir}" --project "$PSScriptRoot\..\${infra_dir}" migrations remove -f

$env:ASPNETCORE_ENVIRONMENT = $prev_env
$env:ESZOP_DB_SEED = "false"