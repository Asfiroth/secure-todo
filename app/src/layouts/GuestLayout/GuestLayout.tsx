import { Outlet } from 'react-router';

import './GuestLayout.css';

export const GuestLayout = () => (
  <main className="guest-layout">
    <Outlet />
  </main>
);

export default GuestLayout;
