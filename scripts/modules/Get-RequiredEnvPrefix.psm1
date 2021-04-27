function Get-RequiredEnvPrefix {
    if (-not($env:ASPNETCORE_ENVIRONMENT)) {
        Write-Error "Environment variable ASPNETCORE_ENVIRONMENT not set" -ErrorAction Stop
    }

    $env_prefix_dict = @{
        "DevHostLocal"   = "dev";
        "DevDockerLocal" = "dev";
        "DevHostCloud"   = "dev";
        "DevDockerCloud" = "dev";
        "Staging"        = "staging";
        "StagingVm"      = "staging";
        "Production"     = "prod";
    }

    $env_prefix = $env_prefix_dict[$env:ASPNETCORE_ENVIRONMENT]
    if (-not($env_prefix)) {
        Write-Error "Invalid environment variable ASPNETCORE_ENVIRONMENT" -ErrorAction Stop
    }

    Write-Output $env_prefix
}

Export-ModuleMember -Function Get-RequiredEnvPrefix