#!/bin/bash
set -e
set -euo pipefail

migrationName=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    -n|--name)
      migrationName="$2"
      shift 2
      ;;
    *)
      echo "Unknown option: $1"
      exit 1
      ;;
  esac
done

if [[ -z "$migrationName" ]]; then
  echo "Error: Migration name is required. Use -n or --name to specify it."
  exit 1
fi

# Create the migration
scriptDir="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

projectFile="${scriptDir}/../api/SecureTodo.Infrastructure/SecureTodo.Infrastructure.csproj"
outputDir="Data/Migrations"
dbContextName="SecureTodoDbContext"
startupProject="${scriptDir}/../api/SecureTodo.Api/SecureTodo.Api.csproj"

echo "Creating migration '${migrationName}'..."

dotnet ef migrations add "${migrationName}" --output-dir "${outputDir}" --context "${dbContextName}" --startup-project "${startupProject}" --project "${projectFile}"
lastExitCode=$?

if [[ $lastExitCode -ne 0 ]]; then
  echo "Error: Failed to create migration. Please check the error message above."
  exit 1
else
  echo "Migration '${migrationName}' created successfully."
fi