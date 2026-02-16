import { createContext } from 'react';
import type { UserInfo } from '~/types/auth.ts';

interface AuthContext {
  isAuthenticated: boolean;
  isLoading: boolean;
  user?: UserInfo;
  token?: string;
  login: () => Promise<void>;
  logout: () => Promise<void>;
  refresh: () => Promise<boolean>;
}

export const AuthContext = createContext<AuthContext>({} as AuthContext);
