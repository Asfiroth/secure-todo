import type { FC, LabelHTMLAttributes } from 'react';
import { type ReactNode } from 'react';

import './Label.css';

interface LabelProps extends LabelHTMLAttributes<HTMLLabelElement> {
  children: ReactNode;
}

export const Label: FC<LabelProps> = ({ children, ...props }) => {
  return (
    <label {...props} className="label">
      {children}
    </label>
  );
};

export default Label;
