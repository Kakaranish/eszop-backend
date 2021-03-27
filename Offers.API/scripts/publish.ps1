param(
    [string] $ImageTag = "latest"
)

docker push "eszopregistry.azurecr.io/eszop-offers-api:$ImageTag"