import axios from 'axios';
import keycloak from './keycloak';

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_TODO_API,
  headers: {
    'Content-Type': 'application/json',
  },
});

apiClient.interceptors.request.use(
  (config) => {
    if (keycloak.authenticated) {
      config.headers.Authorization = `Bearer ${keycloak.token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

apiClient.interceptors.response.use(
  (response) => response,
  async (error) => {
    if (error.response?.status === 401) {
      const refreshed = await keycloak.updateToken(5);
      if (refreshed) {
        error.config.headers.Authorization = `Bearer ${keycloak.token}`;
        return apiClient.request(error.config);
      } else {
        void keycloak.login();
      }
    }
    return Promise.reject(error);
  },
);

export default apiClient;
