param(
    [string] $ImageTag = "latest"
)

docker push "eszopregistry.azurecr.io/eszop-api-gateway:$ImageTag"