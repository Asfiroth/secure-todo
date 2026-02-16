import { Navigate } from 'react-router';
import { Loader } from '~/components/Loader';
import useAuth from '~/hooks/useAuth.ts';
import type { ReactNode } from 'react';

export const ProtectedRoute = ({ children }: { children: ReactNode }) => {
  const { isLoading, isAuthenticated } = useAuth();
  if (isLoading) return <Loader />;
  if (!isAuthenticated) return <Navigate to="/login" replace />;
  return children;
};
