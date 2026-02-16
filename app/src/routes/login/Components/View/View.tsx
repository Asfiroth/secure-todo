import { useNavigate } from 'react-router';
import useAuth from '~/hooks/useAuth';
import { useEffect } from 'react';
import { Loader } from '~/components/Loader';
import './View.css';

const View = () => {
  const { isAuthenticated, isLoading, login } = useAuth();
  const navigate = useNavigate();
  useEffect(() => {
    console.info(isAuthenticated);
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
    <>
      <div>Home</div>
      <button onClick={handleLogin}>Login</button>
    </>
  );
};

export default View;
