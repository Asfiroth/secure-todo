import useAuth from '~/hooks/useAuth.ts';

import './Header.css';
import { ArrowLeftStartOnRectangleIcon } from '@heroicons/react/20/solid';

export const Header = () => {
  const { user, logout } = useAuth();
  return (
    <>
      <div className="welcome-message">
        <span>Welcome, {user?.name}</span>
      </div>
      <button className="logout" onClick={() => logout()}>
        Logout
        <ArrowLeftStartOnRectangleIcon className="h-6 w-6" />
      </button>
    </>
  );
}