#!/bin/bash
set -e
set -euo pipefail
scriptDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

baseDirectory="$scriptDir/../api"
bundleFolder="$baseDirectory/GeneratedBundles"

dbConnectionString="${CONNECTION_STRING}"

if [[ -z "$dbConnectionString" ]]; then
    appsettings="$baseDirectory/SecureTodo.Api/appsettings.Development.json"
    if [ "${ASPNETCORE_ENVIRONMENT}" = "Production" ]; then
        appsettings="$baseDirectory/SecureTodo.Api/appsettings.json"
    fi
    dbConnectionString=$(jq -r '.ConnectionStrings.TodoDb // empty' "$appsettings")
fi

mapfile -t bundleFiles < <(find "$bundleFolder" -maxdepth 1 -type f -name "*_Migrations*")

if [ ${#bundleFiles[@]} -eq 0 ]; then
    echo "No migration bundles found. Exiting..."
    exit 0
fi

for bundlePath in "${bundleFiles[@]}"; do
    if [ ! -e "$bundlePath" ]; then
        echo "Migration bundle not found: $bundlePath"
    fi

    echo "Executing migration bundle: $bundlePath with connection string"

    "$bundlePath" --connection "$dbConnectionString" -v
    
    if [ $? -eq 0 ]; then
        echo "Executed successfully: $(basename "$bundlePath")"
    else
        echo "Error executing $(basename "$bundlePath")"
    fi
done

rm -rf "$bundleFolder"

echo "All migration bundles executed successfully!"