param(
    [string] $ImageTag = "latest"
)

docker build -f $PSScriptRoot/../../Dockerfile -t "eszopregistry.azurecr.io/eszop-api-gateway:$ImageTag" $PSScriptRoot/../../..