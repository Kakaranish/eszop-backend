Import-Module "$PSScriptRoot\Get-RequiredEnvPrefix.psm1" -Force

function Get-AppsConfig {
    param (
        [string] $EnvPrefix
    )
    
    $env_prefix = if ($EnvPrefix) { $EnvPrefix } else { Get-RequiredEnvPrefix }
    
    $config_path = "$PSScriptRoot\..\config\environments\${env_prefix}.yaml"
    $apps_config = Get-Content -Path $config_path | ConvertFrom-Yaml -Ordered

    return $apps_config
}

Export-ModuleMember -Function Get-AppsConfig