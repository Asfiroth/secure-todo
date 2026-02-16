import type { FC } from 'react';

export const Loader: FC = () => {
  return (
    <div className="bubble-loader">
      <div className="bubble bubble-1"></div>
      <div className="bubble bubble-2"></div>
      <div className="bubble bubble-3"></div>
    </div>
  );
};
