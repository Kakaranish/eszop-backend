docker network create eszop-network

docker volume create eszop-servicebus-rabbitmq

docker volume create eszop-offers-sqlserver
docker volume create eszop-identity-sqlserver
docker volume create eszop-identity-redis
docker volume create eszop-carts-sqlserver
docker volume create eszop-orders-sqlserver
docker volume create eszop-notification-sqlserver