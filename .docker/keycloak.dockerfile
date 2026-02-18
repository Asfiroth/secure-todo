FROM --platform=$BUILDPLATFORM keycloak/keycloak:26.3.4 as builder

COPY keycloak/import/*  /opt/keycloak/data/import/