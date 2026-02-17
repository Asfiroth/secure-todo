import type { FC, ReactNode } from 'react';
import './Card.css';

interface CardProps {
  children: ReactNode;
}

export const Card: FC<CardProps> = ({ children }) => {
  return <div className="card">{children}</div>;
};
