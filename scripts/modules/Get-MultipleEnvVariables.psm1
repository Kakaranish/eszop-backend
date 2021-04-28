function Get-MultipleEnvVariables {
  param(
    [string[]] $Variables
  )

  $result = @{}
  foreach ($var_name in $Variables) {
    $var_value = Invoke-Expression "`$env:${var_name}"
    $result.Add($var_name, $var_value)
  }

  return $result
}

Export-ModuleMember -Function Get-MultipleEnvVariables