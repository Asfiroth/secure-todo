FROM --platform=$BUILDPLATFORM node:24-bookworm AS build

WORKDIR /app

COPY . .

RUN npm install
RUN npm run build

FROM nginx:1.29-bookworm AS final

WORKDIR /app/dist

COPY --from=build /app/dist /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

# Create custom user and group
RUN groupadd -r todo_group && useradd --no-log-init -r -g todo_group todo_user 

# Give control to the app folder so it can execute api
RUN chown -R todo_user:todo_group /app

# Use new created user 
USER todo_user

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]
