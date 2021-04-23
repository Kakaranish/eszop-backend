param(
    [string] $ImageTag = "latest",
    [string] $ContainerRepository
)
$scripts_dir = "$PSScriptRoot\..\..\..\..\scripts"
Import-Module "${scripts_dir}\AzureConfig.psm1" -Force

$container_repo = if ($ContainerRepository) { $ContainerRepository } else { $ESZOP_AZURE_CONTAINER_REPO }

docker run `
    --rm `
    -itd `
    -p 7000:80 `
    -e ASPNETCORE_URLS='http://+' `
    -e ASPNETCORE_ENVIRONMENT="DevDockerLocal" `
    -e ESZOP_LOGS_DIR="/logs" `
    -v "$pwd\..\..\..\logs:/logs" `
    --network eszop-network `
    --name eszop-carts-api `
    "${container_repo}/eszop-carts-api:$ImageTag"