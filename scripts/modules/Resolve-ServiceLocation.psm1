function Resolve-ServiceLocation {
  param(
    [Parameter(Mandatory = $true)]
    [string] $ServiceName
  )

  $services = @{
    "gateway"      = "$PSScriptRoot\..\..\API.Gateway\API.Gateway";
    "carts"        = "$PSScriptRoot\..\..\Carts\Carts.API";
    "identity"     = "$PSScriptRoot\..\..\Identity\Identity.API";
    "notification" = "$PSScriptRoot\..\..\NotificationService\NotificationService.API";
    "offers"       = "$PSScriptRoot\..\..\Offers\Offers.API";
    "orders"       = "$PSScriptRoot\..\..\Orders\Orders.API";
  };

  $resolved_path = Resolve-Path $services[$ServiceName]
  Write-Output $resolved_path
}

Export-ModuleMember -Function Resolve-ServiceLocation