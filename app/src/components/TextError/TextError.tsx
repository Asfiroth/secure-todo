import { type FC } from 'react';
import './TextError.css';

interface IProps {
  errorMessage?: string;
}

export const TextError: FC<IProps> = ({ errorMessage }) => {
  if (!errorMessage) return null;
  return <span className='error-message'>{errorMessage}</span>;
};
