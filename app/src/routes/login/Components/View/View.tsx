import { useNavigate } from 'react-router';
import useAuth from '~/hooks/useAuth';
import { useEffect } from 'react';
import { Loader } from '~/components/Loader';
import './View.css';

const View = () => {
  const { isAuthenticated, isLoading, login } = useAuth();
  const navigate = useNavigate();
  useEffect(() => {
    if (isAuthenticated) {
      navigate('/', { replace: true });
    }
  }, [isAuthenticated, navigate]);

  const handleLogin = async () => {
    await login();
  };

  if (isLoading) {
    return <Loader />;
  }

  return (
    <div className="login-container">
      <span className="intro-message">Welcome to Task Manager</span>
      <button className="login-button" onClick={handleLogin}>
        Sign In to continue
      </button>
    </div>
  );
};

export default View;
