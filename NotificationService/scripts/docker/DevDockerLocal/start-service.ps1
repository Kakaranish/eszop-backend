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
    -p 9000:80 `
    -e ASPNETCORE_URLS='http://+' `
    -e ESZOP_LOGS_DIR="/logs" `
    -e ASPNETCORE_ENVIRONMENT="DevDockerLocal" `
    -v "$pwd\..\..\..\logs:/logs" `
    --network eszop-network `
    --name eszop-notification-service `
    "${container_repo}/eszop-notification-service:$ImageTag"