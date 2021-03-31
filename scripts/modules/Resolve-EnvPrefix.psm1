function Resolve-EnvPrefix {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Environment
    )

    $env_prefix_dict = @{
        "DevHostCloud"   = "dev";
        "DevDockerCloud" = "dev";
        "Staging"        = "staging";
    }

    Write-Output $env_prefix_dict[$Environment]
}

Export-ModuleMember -Function Resolve-EnvPrefix