param(
    [Parameter(Mandatory = $true)]
    [string] $ConnectionString,

    [switch] $UseAzureEventBus,

    [switch] $AutoApprove
)

Import-Module $PSScriptRoot\..\modules\Resolve-EnvPrefix.psm1 -Force
Import-Module $PSScriptRoot\..\modules\Resolve-ServiceLocation.psm1 -Force

# ------------------------------------------------------------------------------

if (-not($ConnectionString)) {
    $ConnectionString = $default_connection_string
}

$services = @("carts", "identity", "notification", "offers", "orders")

$environment = $env:ASPNETCORE_ENVIRONMENT
if (-not($environment)) {
    $environment = "DevelopmentLocal"
}
$environment_prefix = Resolve-EnvPrefix -Environment $environment
$appsettings_filename = "appsettings.$environment.json"
$topic_name = "eszop-$environment_prefix-event-bus-topic"
$sub_name_template = "eszop-$environment_prefix-event-bus-{service_name}-sub"

#-------------------------------------------------------------------------------
if (-not($AutoApprove.IsPresent)) {
    Write-Host "Those files will be changed:"
    foreach ($service in $services) {
        $service_location = Resolve-ServiceLocation -ServiceName $service
        Write-Host (Resolve-Path -Path (Join-Path $service_location $appsettings_filename))
    }
    $choice = Read-Host "Do you want to continue (y/n)"
    if ($choice -ne "y") {
        exit
    }
}

foreach ($service in $services) {
    $service_location = Resolve-ServiceLocation -ServiceName $service
    $appsettings_path = Resolve-Path -Path (Join-Path $service_location $appsettings_filename)
    if (-not(Test-Path -Path $appsettings_path)) {
        Write-Warning "[SKIP] File $appsettings_path does not exist"
        continue
    }
    
    $sub_name = $sub_name_template -replace "{service_name}", $service
    $appsettings_json = Get-Content -Path $appsettings_path | ConvertFrom-Json

    $appsettings_json.EventBus.AzureEventBus | Add-Member -Name "ConnectionString" -MemberType NoteProperty -Value $ConnectionString -Force
    $appsettings_json.EventBus.AzureEventBus | Add-Member -Name "TopicName" -MemberType NoteProperty -Value $topic_name -Force
    $appsettings_json.EventBus.AzureEventBus | Add-Member -Name "SubscriptionName" -MemberType NoteProperty -Value $sub_name -Force
    $appsettings_json.EventBus | Add-Member -Name "UseAzureEventBus" -MemberType NoteProperty -Value $UseAzureEventBus.IsPresent -Force

    $appsettings_json | ConvertTo-Json -Depth 9 | Set-Content $appsettings_path
}

Write-Host "appsettings.json files updated successfully"