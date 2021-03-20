docker run `
    --rm `
    -itd `
    -p 7000:80 `
    -e ASPNETCORE_ENVIRONMENT='Development' `
    -e ASPNETCORE_URLS='http://+' `
    --network eszop-network `
    --name eszop-carts-api `
    eszop-carts-api