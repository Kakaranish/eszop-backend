docker run `
    --rm `
    -itd `
    -p 10000:80 `
    -e ASPNETCORE_ENVIRONMENT='Development' `
    -e ASPNETCORE_URLS='http://+' `
    --network eszop-network `
    --name eszop-api-gateway `
    eszop-api-gateway
