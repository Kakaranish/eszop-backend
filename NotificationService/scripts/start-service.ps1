docker run `
    --rm `
    -itd `
    -p 9000:80 `
    -e ASPNETCORE_ENVIRONMENT='Development' `
    -e ASPNETCORE_URLS='http://+' `
    --network eszop-network `
    --name eszop-notification-service `
    eszop-notification-service