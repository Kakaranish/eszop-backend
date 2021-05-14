#!/bin/bash

# ------------------------------------------------------------------------------
#
# This script destroys docker resources created with 
# 'initialize_docker_resources.sh' script.
# 
# ------------------------------------------------------------------------------

read -p "Are you sure? [y/n] " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]] ; then
    exit
fi

docker network rm eszop-network

docker volume rm eszop-servicebus-rabbitmq

docker volume rm eszop-offers-sqlserver
docker volume rm eszop-identity-sqlserver
docker volume rm eszop-identity-redis
docker volume rm eszop-carts-sqlserver
docker volume rm eszop-orders-sqlserver
docker volume rm eszop-notification-sqlserver