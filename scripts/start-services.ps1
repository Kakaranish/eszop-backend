$lookup_dirs = @(
    "..\API.Gateway",
    "..\Carts.API",
    "..\Identity.API",
    "..\NotificationService",
    "..\Offers.API",
    "..\Orders.API" 
);

foreach ($dir in $lookup_dirs) {
    $scripts_dir = Join-Path (Resolve-Path $dir) "scripts"
    $build_script = Join-Path $scripts_dir "start-service.ps1"

    if(Test-Path $build_script) {
        Write-Host "Run $build_script"
        & $build_script
    }
}
