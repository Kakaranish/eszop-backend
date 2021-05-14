#!/bin/bash

# ------------------------------------------------------------------------------
#
# This script starts dependent services for eszop applications.
# Those services are:
# - SQL Server DBs - for apps: Offers, Identity, Carts, Orders, Notification
# - RedisDb - for apps: Identity
# - RabbitMq - for apps: Offers, Identity, Carts, Orders, Notification
# 
# NOTE:
# When first run, use '--applyMigrations' flag to apply migrations to newly created
# databases. Without this applications will throw exceptions.
#
# ------------------------------------------------------------------------------

for arg in $@ ; do
    if [ "$1" == "--applyMigrations" ] ; then
        applyMigrations=true
    fi
done

if [ $applyMigrations ] ; then
    echo "Apply migrations"
fi

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" &>/dev/null && pwd)"
items=$(find $SCRIPT_DIR | grep "start_dependent_services.sh")

for item in $items ; do
    if [ "$item" == "$SCRIPT_DIR/start_dependent_services.sh" ] ; then
        continue
    fi

    . $item
done

# TODO: Apply migrations