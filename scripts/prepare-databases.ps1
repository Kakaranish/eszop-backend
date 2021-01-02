$env:ASPNETCORE_ENVIRONMENT="DevelopmentLocal"

Set-Location -Path (Join-Path -Path $PSScriptRoot ".." "Identity.API")
dotnet ef database update

Set-Location -Path (Join-Path -Path $PSScriptRoot ".." "Offers.API")
dotnet ef database update

Set-Location -Path (Join-Path -Path $PSScriptRoot ".." "Orders.API")
dotnet ef database update

Set-Location -Path (Join-Path -Path $PSScriptRoot ".." "Carts.API")
dotnet ef database update

Set-Location $PSScriptRoot