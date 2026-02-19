FROM --platform=$BUILDPLATFORM keycloak/keycloak:26.3.4

# Copy the realm configuration file into the Keycloak import directory
COPY /import/  /opt/keycloak/data/import/