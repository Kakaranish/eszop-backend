docker run `
    --rm `
    -itd `
    -p 5000:80 `
    -p 5001:8080 `
    -e ASPNETCORE_ENVIRONMENT='Development' `
    -e ASPNETCORE_URLS='http://+' `
    --network eszop-network `
    --name eszop-offers-api `
    eszop-offers-api