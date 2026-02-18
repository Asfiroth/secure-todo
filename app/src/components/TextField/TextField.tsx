import { forwardRef, type InputHTMLAttributes, type ReactNode, useState } from 'react';
import './TextField.css';

interface TextFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  action?: ReactNode;
}

export type Ref = HTMLInputElement;

export const TextField = forwardRef<Ref, TextFieldProps>(({ action, ...props }, ref) => {
  const [isFocused, setIsFocused] = useState<boolean>(false);
  return (
    <div className={`text-field ${isFocused ? 'focus' : ''}`}>
      <input
        {...props}
        ref={ref}
        onFocus={() => setIsFocused(true)}
        onBlur={() => setIsFocused(false)}
      />
      {action}
    </div>
  );
});

TextField.displayName = 'TextField';

export default TextField;
