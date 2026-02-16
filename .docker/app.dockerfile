FROM --platform=$BUILDPLATFORM node:24-bookworm AS build

WORKDIR /app

COPY . .

RUN npm install
RUN npm run build

FROM nginxinc/nginx-unprivileged:1.29-bookworm AS final

WORKDIR /app/dist

COPY --from=build /app/build/client /usr/share/nginx/html
COPY --from=build /app/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 8080

CMD ["nginx", "-g", "daemon off;"]
