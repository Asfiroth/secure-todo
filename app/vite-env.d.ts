/// <reference types="vite/client" />

interface ImportMetaEnv {
  VITE_KEYCLOAK_URL: string;
  VITE_KEYCLOAK_REALM: string;
  VITE_KEYCLOAK_CLIENT_ID: string;
  VITE_TODO_API: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
