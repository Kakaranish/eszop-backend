# Environments 

Each name of development environment consists of 3 parts:

```
Dev   Host/Docker   Local/Cloud
^^^   ^^^^^^^^^^^   ^^^^^^^^^^^               
(1)       (2)           (3)

(1) -> environment prefix
(2) -> Host = apps run on host OS using `dotnet` CLI or Visual Studio
    -> Docker = apps run in docker containers
(3) -> Local = apps use local resources (DBs, Message Broker)
    -> Cloud = apps use cloud resources (DBs, PubSub)
```

# Local Environments
Requirements: <br>
-Dependent services must be started before running eszop applications <br>
-Migrations must be applied to databases before first run

## DevHostLocal
-When running using Visual Studio: no additional configuration needed <br>
-When running with dotnet CLI: Environment variable `ASPNETCORE_ENVIRONMENT` must be set to `DevHostLocal` .

## DevDockerLocal
All eszop apps can be started using `Start-Services.ps1` powershell script.

# Cloud environments

General info: <br>
-There are 3 cloud environments: `dev`, `staging`, `cloud` <br>
-Before running apps on given environment, configuration file `config/environments/{env}.yaml` must be filled in <br>
-Latest migrations must be applied to cloud databases <br>

## DevHostCloud
Starting apps on this environment **requires Visual Studio**. Before first run, it's neccessary to call `Set-DevHostCloud_VisualStudio.ps1` powershell script. Then for each app project `{APP_NAME}.Cloud` run configuration in VS should be chosen. For example: <br>
-For `API.Gateway` `Api.Gateway.Cloud` configuration should be chosen <br>
-For `Offers.API` `Offers.API.Cloud` configuration should be chosen

## DevDockerCloud
All eszop apps can be started using `Start-Services.ps1` powershell script.