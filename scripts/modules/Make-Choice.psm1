function Make-Choice {
  param (
    [Parameter(Mandatory = $true)]
    [string] $Title,

    [Parameter(Mandatory = $true)]
    [string[]] $Choices
  )

  Write-Host $Title
  For ($i = 0; $i -lt $Choices.Count; $i++) {
    Write-Host "$($i+1): $($Choices[$i])"
  }

  [int]$choice = Read-Host "Press the number to make choice"

  if ($choice -lt 1 -or $choice -gt $Choices.Count) {
    Write-Error "Invalid choice" -ErrorAction Stop
    exit
  }

  Write-Output $Choices[$choice - 1]
}

Export-ModuleMember -Function Make-Choice
