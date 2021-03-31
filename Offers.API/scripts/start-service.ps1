param(
    [string] $ImageTag = "latest"
)

$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "Development"
}

$event_bus_conn_str = $env:ESZOP_AZURE_EVENTBUS_CONN_STR

$logs_dir = $env:ESZOP_LOGS_DIR
if(-not($logs_dir)) {
    $logs_dir = "/logs"
}

docker run `
    --rm `
    -itd `
    -p 5000:80 `
    -p 5001:8080 `
    -e ASPNETCORE_ENVIRONMENT="$environment" `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$logs_dir" `
    -e ESZOP_AZURE_EVENTBUS_CONN_STR="$event_bus_conn_str" `
    -v "$pwd\..\logs:/logs" `
    --network eszop-network `
    --name eszop-offers-api `
    "eszopregistry.azurecr.io/eszop-offers-api:$ImageTag"