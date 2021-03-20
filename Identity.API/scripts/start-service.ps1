docker run `
    --rm `
    -itd `
    -p 6000:80 `
    -p 6001:8080 `
    -e ASPNETCORE_ENVIRONMENT='Development' `
    -e ASPNETCORE_URLS='http://+' `
    --network eszop-network `
    --name eszop-identity-api `
    eszop-identity-api