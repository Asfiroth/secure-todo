import { Outlet } from 'react-router';
import { Toaster } from 'react-hot-toast';
import { ProtectedRoute } from '~/hoc/ProtectedRoute.tsx';
import { Header } from '~/components/Header';

import './AuthorizedLayout.css';

export const AuthorizedLayout = () => {
  return (
    <ProtectedRoute>
      <main className="authorized-layout">
        <div className="header-container">
          <Header />
        </div>
        <div className="page-content">
          <Outlet />
        </div>
        <Toaster />
      </main>
    </ProtectedRoute>
  );
};

export default AuthorizedLayout;
