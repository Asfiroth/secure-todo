FROM --platform=$BUILDPLATFORM keycloak/keycloak:26.3.4

COPY /import/  /opt/keycloak/data/import/