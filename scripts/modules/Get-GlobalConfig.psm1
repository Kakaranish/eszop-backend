function Get-GlobalConfig {
  $config_path = "$PSScriptRoot\..\config\global.yaml"
  $global_config = Get-Content -Path $config_path | ConvertFrom-Yaml -Ordered

  return $global_config
}

Export-ModuleMember -Function Get-GlobalConfig