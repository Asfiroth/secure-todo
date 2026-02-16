import React, { useEffect, useState } from 'react';
import type { UserInfo } from '~/types/auth.ts';
import { AuthContext } from '~/contexts/AuthContext.ts';
import keycloak from '~/lib/keycloak.ts';

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [user, setUser] = useState<UserInfo | undefined>();
  const [token, setToken] = useState<string | undefined>();

  const login = async () => {
    await keycloak.login();
  };
  const logout = async () => {
    await keycloak.logout({ redirectUri: window.location.origin });
  };
  const refresh = async () => {
    return await keycloak.updateToken(5);
  };

  useEffect(() => {
    if (keycloak.didInitialize) return;
    keycloak
      .init({
        onLoad: 'check-sso',
        pkceMethod: 'S256',
        silentCheckSsoRedirectUri: window.location.origin + '/silent-check-sso.html',
      })
      .then((authenticated) => {
        console.log('keycloak initialized');
        setIsAuthenticated(authenticated);
        if (authenticated) {
          setUser(keycloak.tokenParsed as UserInfo);
          setToken(keycloak.token);
        }
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        setIsLoading(false);
      });

    keycloak.onTokenExpired = () => {
      keycloak.updateToken(70).then((refreshed) => {
        if (refreshed) {
          setToken(keycloak.token);
        } else {
          void login();
        }
      });
    };
  }, []);

  return (
    <AuthContext.Provider
      value={{
        isAuthenticated,
        isLoading,
        user,
        token,
        login,
        logout,
        refresh,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};
