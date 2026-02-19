# Use the official .NET SDK for building
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
ARG TARGETARCH

# tools scripts need 
RUN apt-get update \
  && apt-get install -y --no-install-recommends jq ca-certificates \
  && rm -rf /var/lib/apt/lists/*

# Install EF CLI tool
ENV DOTNET_TOOLS=/root/.dotnet/tools
ENV PATH="${PATH}:${DOTNET_TOOLS}"
RUN dotnet tool install --global dotnet-ef

# Copy all the project files from the current context
COPY /scripts /scripts
COPY /api /api

# Restore dependencies
RUN dotnet restore api/SecureTodo.Api/SecureTodo.Api.csproj -a $TARGETARCH

# Ensure scripts are executable
RUN chmod +x ./scripts/*.sh

# Run bundle + execute in one shot
ENTRYPOINT ["bash", "-lc", "./scripts/bundle_migration.sh && ./scripts/execute_bundle.sh"]