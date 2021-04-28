function Prepare-SqlConnectionString {
  param (
    [string] $EnvironmentPrefix,
    [string] $ServiceName,
    [string] $DbUsername,
    [string] $DbPassword
  )
    
  $ESZOP_SQLSERVER_CONN_STR_template = "Server=tcp:eszop-{env_prefix}-sqlserver.database.windows.net,1433;Initial Catalog=eszop-{env_prefix}-{service_name}-db;Persist Security Info=False;User ID={db_username};Password={db_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

  Write-Output ($ESZOP_SQLSERVER_CONN_STR_template `
      -replace "{env_prefix}", $EnvironmentPrefix `
      -replace "{service_name}", $ServiceName `
      -replace "{db_username}", $DbUsername `
      -replace "{db_password}", $DbPassword)
}

Export-ModuleMember -Function Prepare-SqlConnectionString