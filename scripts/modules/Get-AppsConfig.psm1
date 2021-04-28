function Get-AppsConfig {
  param (
    [Parameter(Mandatory = $true)]  
    [ValidateSet("dev", "staging", "prod")]
    [string] $CloudEnv
  )

  $config_path = "$PSScriptRoot\..\config\environments\$CloudEnv.yaml"
  $apps_config = Get-Content -Path $config_path | ConvertFrom-Yaml -Ordered

  return $apps_config
}

Export-ModuleMember -Function Get-AppsConfig