Import-Module "$PSScriptRoot\Make-Choice.psm1" -DisableNameChecking

function Require-CloudEnvVar {
    $cloud_envs = @(
        "dev"
        "staging",
        "prod"
    )

    if ($cloud_envs.Contains($env:ESZOP_CLOUD_ENV)) {
        return $env:ESZOP_CLOUD_ENV
    }

    return Make-Choice `
        -Title "Choose cloud environment" `
        -Choices $cloud_envs
}

Export-ModuleMember -Function Require-CloudEnvVar