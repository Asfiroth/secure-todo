# Use the official .NET SDK for building
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
ARG TARGETARCH
WORKDIR /src

# Copy all the project files from the current context (which is already ./src/)
COPY . .

# Restore dependencies
RUN dotnet restore SecureTodo.Api/SecureTodo.Api.csproj -a $TARGETARCH

# Build the project
RUN dotnet build SecureTodo.Api/SecureTodo.Api.csproj -c Release -a $TARGETARCH --no-restore 

# Publish the project
RUN dotnet publish SecureTodo.Api/SecureTodo.Api.csproj -c Release -a $TARGETARCH --no-restore --no-build --self-contained false -o /app/publish

# Use a lightweight runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS final
WORKDIR /app
COPY --from=build /app/publish .

# Create custom user and group
RUN groupadd -r todo_group && useradd --no-log-init -r -g todo_group todo_user 

# Give control to the app folder so it can execute api
RUN chown -R todo_user:todo_group /app

# Use new created user 
USER todo_user

# Expose the port the app runs on
EXPOSE 8080

# Set the entry point
ENTRYPOINT ["dotnet", "SecureTodo.Api.dll"]
