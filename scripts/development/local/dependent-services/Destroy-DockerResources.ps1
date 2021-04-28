# ------------------------------------------------------------------------------
#
# This script destroys docker resources created with 
# 'Initialize-DockerResources.ps1' script.
# 
# ------------------------------------------------------------------------------

$reply = Read-Host -Prompt "All databases data will be lost. Continue? [y/n]"
if ( -not($reply -match "[yY]") ) { 
  exit
}

docker network rm eszop-network

docker volume rm eszop-servicebus-rabbitmq

docker volume rm eszop-offers-sqlserver
docker volume rm eszop-identity-sqlserver
docker volume rm eszop-identity-redis
docker volume rm eszop-carts-sqlserver
docker volume rm eszop-orders-sqlserver
docker volume rm eszop-notification-sqlserver