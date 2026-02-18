FROM --platform=$BUILDPLATFORM keycloak/keycloak:26.3.4

COPY keycloak/import/*  /opt/keycloak/data/import/