#!/bin/bash

# ------------------------------------------------------------------------------
#
# This script stops dependent services which were started with
# 'start_dependent_services.sh' script.
#
# ------------------------------------------------------------------------------

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" &>/dev/null && pwd)"
items=$(find $SCRIPT_DIR | grep "stop_dependent_services.sh")

for item in $items ; do
    if [ "$item" == "$SCRIPT_DIR/stop_dependent_services.sh" ] ; then
        continue
    fi

    . $item
done