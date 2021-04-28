# ------------------------------------------------------------------------------
#
# This script initializes docker resources that are required before
# 'Start-DependentServices.ps1' execution.
# There is created docker network to which all dependent services and applications
# are connected. There are also created docker volumes which are used as 
# persistent storage for databases.
#
# ------------------------------------------------------------------------------

docker network create eszop-network

docker volume create eszop-servicebus-rabbitmq

docker volume create eszop-offers-sqlserver
docker volume create eszop-identity-sqlserver
docker volume create eszop-identity-redis
docker volume create eszop-carts-sqlserver
docker volume create eszop-orders-sqlserver
docker volume create eszop-notification-sqlserver