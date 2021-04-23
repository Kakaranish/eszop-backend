## How to start services first time?
#### 1) Initialize docker resources with script
```
.\dependent-services\init-docker-resources.ps1
```

#### 2) Start dependent services (DBs and EventBus) and apply migrations
```
.\dependent-services\start-dependent-services.ps1 -ApplyMigrations
```

#### 3) Start services
```
.\DevDockerLocal\start-services.ps1 -ImageTag latest
```

## How to start services later?

#### 1) Make sure that dependent services (DBs and EventBus) are started. If not start them using:
```
.\dependent-services\start-dependent-services.ps1
```

#### 2) Start services
```
.\DevDockerLocal\start-services.ps1 -ImageTag latest
```