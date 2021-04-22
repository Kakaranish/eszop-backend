param(
    [string] $ImageTag = "latest",
    [string] $ContainerRepository
)

Import-Module "$PSScriptRoot\..\..\..\scripts\AzureConfig.psm1" -Force

$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $ESZOP_AZURE_CONTAINER_REPO }

docker push "${container_repo}/eszop-orders-api:$ImageTag"