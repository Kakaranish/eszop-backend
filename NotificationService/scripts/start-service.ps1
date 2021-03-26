$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "Development"
}

$logs_dir = $env:ESZOP_LOGS_DIR
if(-not($logs_dir)) {
    $logs_dir = "/logs"
}

docker run `
    --rm `
    -itd `
    -p 9000:80 `
    -e ASPNETCORE_ENVIRONMENT="$environment" `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$logs_dir" `
    -v "$pwd\..\logs:/logs" `
    --network eszop-network `
    --name eszop-notification-service `
    eszop-notification-service