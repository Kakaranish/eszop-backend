docker run `
    --rm `
    -itd `
    -e ACCEPT_EULA=s `
    -e MSSQL_PID=Developer `
    -e MSSQL_SA_PASSWORD=Test1234 `
    -p 6100:1433 `
    -v eszop-identity-sqlserver:/var/opt/mssql `
    --name eszop-identity-sqlserver `
    --network eszop-network `
    mcr.microsoft.com/mssql/server:2019-latest

docker run `
    --rm `
    -itd `
    -p 6200:6379 `
    -v eszop-identity-redis:/bitnami/redis/data `
    -e REDIS_PASSWORD=Test1234 `
    --name eszop-identity-redis `
    --network eszop-network `
    bitnami/redis:latest