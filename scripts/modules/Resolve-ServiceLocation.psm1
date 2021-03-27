function Resolve-ServiceLocation {
    param(
        [Parameter(Mandatory = $true)]
        [string] $ServiceName
    )

    $services = @{
        "gateway" = "$PSScriptRoot\..\..\API.Gateway";
        "carts" = "$PSScriptRoot\..\..\Carts.API";
        "identity" = "$PSScriptRoot\..\..\Identity.API";
        "notification" = "$PSScriptRoot\..\..\NotificationService";
        "offers" = "$PSScriptRoot\..\..\Offers.API";
        "orders" = "$PSScriptRoot\..\..\Orders.API";
    };

    $resolved_path = Resolve-Path $services[$ServiceName]
    Write-Output $resolved_path
}

Export-ModuleMember -Function Resolve-ServiceLocation