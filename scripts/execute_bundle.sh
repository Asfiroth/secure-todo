#!/bin/bash
set -e
set -euo pipefail
scriptDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

baseDirectory="$scriptDir/../api"
appSettingsSource="$baseDirectory/SecureTodo.Api/appsettings.Development.json"
bundleFolder="$baseDirectory/GeneratedBundles"

# if [ "$environment" = "Production" ]; then
#     appSettingsSource="$baseDirectory/SecureTodo.Api/appsettings.json"
# fi

if [ ! -e "$appSettingsSource" ]; then
    echo "appsettings.json not found in SecureTodo.Api. Exiting..."
    exit 1
fi

cp -f "$appSettingsSource" "$bundleFolder"
echo "Copied appsettings.json to: $bundleFolder"

mapfile -t bundleFiles < <(find "$bundleFolder" -maxdepth 1 -type f -name "*_Migrations*")

if [ ${#bundleFiles[@]} -eq 0 ]; then
    echo "No migration bundles found. Exiting..."
    exit 1
fi

# if [ "$environment" = "Development" ]; then
#     # Load the connection strings from appsettings.json using jq
#     connectionString=$(jq -r '.ConnectionStrings.TodoDb' "$appSettingsSource")
# fi

connectionString=$(jq -r '.ConnectionStrings.TodoDb' "$appSettingsSource")

# Set environment variable to ensure only WriteDbContext is used
##export ASPNETCORE_ENVIRONMENT="$environment"

for bundlePath in "${bundleFiles[@]}"; do
    if [ ! -e "$bundlePath" ]; then
        echo "Migration bundle not found: $bundlePath"
    fi

    echo "Executing migration bundle: $bundlePath with connection string"

    "$bundlePath" --connection "$connectionString" -v
    if [ $? -eq 0 ]; then
        echo "Executed successfully: $(basename "$bundlePath")"
    else
        echo "Error executing $(basename "$bundlePath")"
    fi
done

##unset ASPNETCORE_ENVIRONMENT

rm -rf "$bundleFolder"

echo "All migration bundles executed successfully!"