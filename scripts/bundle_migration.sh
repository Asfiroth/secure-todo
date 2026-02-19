#!/bin/bash
set -e
set -euo pipefail

scriptDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

projectFile="${scriptDir}/../api/SecureTodo.Infrastructure/SecureTodo.Infrastructure.csproj"
dbContextName="SecureTodoDbContext"
startupProject="${scriptDir}/../api/SecureTodo.Api/SecureTodo.Api.csproj"
outputFile="${scriptDir}/../api/GeneratedBundles/SecureTodo_Migrations"

dbConnectionString="${CONNECTION_STRING}"

if [[ -z "$dbConnectionString" ]]; then
    appsettings="$baseDirectory/SecureTodo.Api/appsettings.Development.json"
    if [ "${ASPNETCORE_ENVIRONMENT}" = "Production" ]; then
        appsettings="$baseDirectory/SecureTodo.Api/appsettings.json"
    fi
    dbConnectionString=$(jq -r '.ConnectionStrings.TodoDb // empty' "$appsettings")
fi

echo "Starting migration bundle generation process..."

# Get migration list
migrationsList=$(dotnet ef migrations list --context "$dbContextName" --project "$projectFile" --startup-project "$startupProject" --connection "$dbConnectionString" 2>&1)
lastExitCode=$?

if [[ $lastExitCode -ne 0 ]]; then
    echo "ERROR: Failed to retrieve migrations list"
    exit 1
fi

# Find if there's pending migrations (lines matching 14 digits + _ + name + (Pending))
latestPendingMigrations=$(echo "$migrationsList" | grep -E "^[0-9]{14}_.+\s+\(Pending\)")

echo "Migrations list :"
echo "$latestPendingMigrations"

# Get the latest pending migration (last line)
latestPendingMigration=$(echo "$latestPendingMigrations" | tail -n 1)

if [[ -z "$latestPendingMigration" ]]; then
    echo "No pending migrations found for context: $dbContextName"
    exit 0
fi

echo "Generating migration bundle -> Output: $outputFile -> DbContext: $dbContextName"

# Build the command
command=(dotnet ef migrations bundle --output "$outputFile" --context "$dbContextName" --startup-project "$startupProject" --project "$projectFile")

os_type="$(uname -s)"
arch="$(uname -m)"

# default: don't force; let dotnet decide when local
target_runtime=""
case "${os_type}_${arch}" in
  Linux_x86_64|Darwin_x86_64)
    target_runtime="linux-x64"
    ;;
  Linux_aarch64|Linux_arm64|Darwin_arm64)
    target_runtime="linux-arm64"
    ;;
esac

if [[ -n "$target_runtime" ]]; then
    command+=(--target-runtime "$target_runtime" --self-contained)
fi

"${command[@]}"
lastExitCode=$?

if [ $lastExitCode -ne 0 ]; then
    echo ""
    echo "ERROR: Failed to generate migration bundle"
    exit 1
fi

echo "Generated Migrations Bundle: $outputFile"