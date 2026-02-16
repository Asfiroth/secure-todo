import './GuestLayout.css';
import { Outlet } from 'react-router';
import { Toaster } from 'react-hot-toast';
import type { ReactNode } from 'react';

const Layout = ({ children }: { children: ReactNode }) => {
  return <main className="guest-layout">{children}</main>;
};

export const GuestLayout = () => (
  <>
    <Layout>
      <Outlet />
    </Layout>
    <Toaster />
  </>
);

export default GuestLayout;
