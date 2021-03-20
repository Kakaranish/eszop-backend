$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "Development"
}

docker run `
    --rm `
    -itd `
    -p 5000:80 `
    -p 5001:8080 `
    -e ASPNETCORE_ENVIRONMENT="$environment" `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$env:ESZOP_LOGS_DIR" `
    -v "$pwd\..\logs:/logs" `
    --network eszop-network `
    --name eszop-offers-api `
    eszop-offers-api