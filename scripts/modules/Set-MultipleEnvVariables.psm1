function Set-MultipleEnvVariables {
  param(
    [hashtable] $EnvDictionary
  )

  foreach ($env_var in $EnvDictionary.Keys) {
    Invoke-Expression "`$env:${env_var}=`"$($EnvDictionary[$env_var])`""
  }
}

Export-ModuleMember -Function Set-MultipleEnvVariables