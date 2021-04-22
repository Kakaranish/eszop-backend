param(
    [string] $ImageTag = "latest",
    [string] $ContainerRepository
)

Import-Module "$PSScriptRoot\..\..\..\scripts\AzureConfig.psm1" -Force

$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $ESZOP_AZURE_CONTAINER_REPO }

docker build -f $PSScriptRoot/../../Dockerfile -t "${container_repo}/eszop-carts-api:$ImageTag" $PSScriptRoot/../../..