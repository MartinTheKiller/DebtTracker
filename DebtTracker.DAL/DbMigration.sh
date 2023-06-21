#!/usr/bin/env bash

DOTNETEF="dotnet-ef.exe"
declare -a CONTEXTS=( "SqLiteDbContext" "SqlServerDbContext" )
declare -a OUTPUT_DIRS=( "./Migrations/SqLite" "./Migrations/SqlServer" )

function print_help () {
    echo "ERR: Wrong arguments!"
    echo "Usage: $0 OPERATION [NAME]"
    echo "          OPERATION - Can be one of: add | remove | update"
    echo "                    - add | remove : Adds/removes migration"
    echo "                    - update : Updates the databases to their last migration"
    echo "          NAME - Name of migration when adding a new one"
}

if [[ $# -lt 1 || $# -gt 2 ]]; then
    print_help
    exit 1
elif [[ "$1" = "add" && $# -ne 2 ]]; then
    print_help
    exit 1
elif [[ "$1" != "add" && $# -ne 1 ]]; then
    print_help
    exit 1
fi

case $1 in

    "add")
        for i in $(seq 0 $(expr ${#CONTEXTS[@]} - 1))
        do
            echo "#### $DOTNETEF migrations add \"$2\" --context \"${CONTEXTS[$i]}\" --output-dir \"${OUTPUT_DIRS[$i]}\" ####"
            eval "$DOTNETEF migrations add \"$2\" --context \"${CONTEXTS[$i]}\" --output-dir \"${OUTPUT_DIRS[$i]}\""
        done
        ;;

    "remove")
        for i in $(seq 0 $(expr ${#CONTEXTS[@]} - 1))
        do
            echo "#### $DOTNETEF migrations remove --context \"${CONTEXTS[$i]}\" ####"
            eval "$DOTNETEF migrations remove --context \"${CONTEXTS[$i]}\""
        done
        ;;

    "update")
        for i in $(seq 0 $(expr ${#CONTEXTS[@]} - 1))
        do
            echo "#### $DOTNETEF database update --context \"${CONTEXTS[$i]}\" ####"
            eval "$DOTNETEF database update --context \"${CONTEXTS[$i]}\""
        done
        ;;
    
    *)
        print_help
        exit 1
        ;;
esac