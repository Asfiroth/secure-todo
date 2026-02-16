export interface UserInfo {
  sub: string;
  email: string;
  name: string;
  given_name: string;
  family_name: string;
}

export interface AuthState {
  isAuthenticated: boolean;
  isLoading: boolean;
  user: UserInfo | null;
  token: string | null;
  refreshToken: string | null;
}
