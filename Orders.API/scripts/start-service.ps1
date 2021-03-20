docker run `
    --rm `
    -itd `
    -p 8000:80 `
    -p 8001:8080 `
    -e ASPNETCORE_ENVIRONMENT='Development' `
    -e ASPNETCORE_URLS='http://+' `
    --network eszop-network `
    --name eszop-orders-api `
    eszop-orders-api