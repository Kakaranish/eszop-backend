docker run `
    --rm `
    -itd `
    -p 10000:80 `
    -e ASPNETCORE_ENVIRONMENT='Development' `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="$env:ESZOP_LOGS_DIR" `
    -v "$pwd\..\logs:/logs" `
    --network eszop-network `
    --name eszop-api-gateway `
    eszop-api-gateway
