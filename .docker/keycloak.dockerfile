FROM quay.io/keycloak/keycloak:26.0.5
USER root

RUN mkdir -p /opt/keycloak/data/import && \
    chown -R keycloak:root /opt/keycloak/data/import && \
    chmod -R 775 /opt/keycloak/data/import

USER keycloak