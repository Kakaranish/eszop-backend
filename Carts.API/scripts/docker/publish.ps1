param(
    [string] $ImageTag = "latest"
)

docker push "eszopregistry.azurecr.io/eszop-carts-api:$ImageTag"