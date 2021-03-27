function Resolve-EnvPrefix {
    param(
        [Parameter(Mandatory = $true)]
        [string] $Environment
    )

    $env_lowercase = $Environment.ToLowerInvariant()

    $env_prefix_dict = @{
        "development"      = "dev";
        "developmentlocal" = "dev-local";
        "staging"          = "staging"
    }

    Write-Output $env_prefix_dict[$env_lowercase]
}

Export-ModuleMember -Function Resolve-EnvPrefix