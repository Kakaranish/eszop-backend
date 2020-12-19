Create network
```
docker network create eszop-network
```

Create volumes
```
docker volume create eszop-products-sqlserver
docker volume create eszop-identity-sqlserver
docker volume create eszop-identity-redis
```
\
Run Products.API SQL Server
```
docker run \
    --rm \
    -it \
    -e ACCEPT_EULA=s \
    -e MSSQL_PID=Developer \
    -e MSSQL_SA_PASSWORD=Test1234 \
    -p 5100:1433 \
    -v eszop-products-sqlserver:/var/opt/mssql \
    --name eszop-products-sqlserver \
    --network eszop-network \
    mcr.microsoft.com/mssql/server:2019-latest
```

Run Identity.API SQL Server
```
docker run \
    --rm \
    -it \
    -e ACCEPT_EULA=s \
    -e MSSQL_PID=Developer \
    -e MSSQL_SA_PASSWORD=Test1234 \
    -p 6100:1433 \
    -v eszop-identity-sqlserver:/var/opt/mssql \
    --name eszop-identity-sqlserver \
    --network eszop-network \
    mcr.microsoft.com/mssql/server:2019-latest
```
Run Identity.API Redis Cache
```
docker run \
    --rm \
    -it \
    -p 6200:6379 \
    -v eszop-identity-redis:/bitnami/redis/data \
    -e REDIS_PASSWORD=Test1234 \
    --name eszop-identity-redis \
    --network eszop-network \
    bitnami/redis:latest
```