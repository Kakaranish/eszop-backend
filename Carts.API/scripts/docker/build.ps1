param(
    [string] $ImageTag = "latest"
)

docker build -f $PSScriptRoot/../../Dockerfile -t "eszopregistry.azurecr.io/eszop-carts-api:$ImageTag" $PSScriptRoot/../../..