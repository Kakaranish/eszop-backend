function Require-EnvironmentVariables {
  param(
    [string[]] $EnvironmentVariables
  )

  $missing_env_vars = @()
  foreach ($required_env_variable in $EnvironmentVariables) {
    $val = Invoke-Expression ('$env:' + $required_env_variable) 
    if (-not($val)) { 
      $missing_env_vars += $required_env_variable
    }
  }

  if ($missing_env_vars.Count) {
    Write-Error "Missing environment variables: $missing_env_vars" -ErrorAction Stop
  }    
}

Export-ModuleMember -Function Require-EnvironmentVariables