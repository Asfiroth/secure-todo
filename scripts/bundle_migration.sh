#!/bin/bash
set -e
set -euo pipefail

scriptDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

projectFile="${scriptDir}/../api/SecureTodo.Infrastructure/SecureTodo.Infrastructure.csproj"
dbContextName="SecureTodoDbContext"
startupProject="${scriptDir}/../api/SecureTodo.Api/SecureTodo.Api.csproj"
outputFile="${scriptDir}/../api/GeneratedBundles/SecureTodo_Migrations"

echo "Starting migration bundle generation process..."
# Get migration list

migrationsList=$(dotnet ef migrations list --context "$dbContextName" --project "$projectFile" --startup-project "$startupProject" 2>&1)
lastExitCode=$?

if [[ $lastExitCode -ne 0 ]]; then
    echo "ERROR: Failed to retrieve migrations list"
    exit 1
fi

# Find latest pending migrations (lines matching 14 digits + _ + name + (Pending))
latestPendingMigrations=$(echo "$migrationsList" | grep -E "^[0-9]{14}_.+\s+\(Pending\)")

echo "Migrations list :"
echo "$latestPendingMigrations"

# Get the latest pending migration (last line)
latestPendingMigration=$(echo "$latestPendingMigrations" | tail -n 1)

if [[ -z "$latestPendingMigration" ]]; then
    echo "No migrations found for context: $dbContextName"
    exit 0
fi

# Extract the migration name (remove trailing " (Pending)")
latestPendingMigration=$(echo "$latestPendingMigration" | sed 's/ \+(Pending)//')

echo "Generating migration bundle -> Output: $outputFile -> DbContext: $dbContextName"

# Build the command
command=(dotnet ef migrations bundle --output "$outputFile" --context "$dbContextName" --startup-project "$startupProject" --project "$projectFile")

os_type=$(uname -s)

if [[ "$os_type" == "Linux" || "$os_type" == "Darwin" ]]; then
    command+=(--target-runtime "linux-x64" --self-contained)
fi

"${command[@]}"
lastExitCode=$?

if [ $lastExitCode -ne 0 ]; then
    echo ""
    echo "ERROR: Failed to generate migration bundle"
    exit 1
fi

echo "Generated Migrations Bundle: $outputFile"