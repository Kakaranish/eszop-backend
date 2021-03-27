param(
    [string] $ImageTag = "latest"
)

docker push "eszopregistry.azurecr.io/eszop-orders-api:$ImageTag"