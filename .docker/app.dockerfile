# Use an official Node.js image as the build stage
FROM --platform=$BUILDPLATFORM node:24-bookworm AS build

# Add build arguments for vite environment variables
ARG KEYCLOAK_URL
ARG KEYCLOAK_REALM
ARG KEYCLOAK_CLIENT_ID
ARG TODO_API

# Set the working directory
WORKDIR /app

COPY . .

# Install dependencies and build the React app with the provided environment variables
ENV VITE_KEYCLOAK_URL=${KEYCLOAK_URL}
ENV VITE_KEYCLOAK_REALM=${KEYCLOAK_REALM}
ENV VITE_KEYCLOAK_CLIENT_ID=${KEYCLOAK_CLIENT_ID}
ENV VITE_TODO_API=${TODO_API}
RUN npm install
RUN npm run build

# Use the official Nginx unprivileged image for the final stage
FROM nginxinc/nginx-unprivileged:1.29-bookworm AS final

WORKDIR /app/dist

COPY --from=build /app/build/client /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]
