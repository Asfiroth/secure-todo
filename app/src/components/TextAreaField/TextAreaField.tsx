import { forwardRef, type ReactNode, type TextareaHTMLAttributes, useState } from 'react';
import './TextAreaField.css';

interface IProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  action?: ReactNode;
}

export type Ref = HTMLTextAreaElement;

export const TextAreaField = forwardRef<Ref, IProps>(
  ({ action, ...props }, ref) => {
    const [isFocused, setIsFocused] = useState<boolean>(false);
    return (
      <div className={`text-area ${isFocused ? 'focus' : ''}`}>
        <textarea
          {...props}
          ref={ref}
          onFocus={() => setIsFocused(true)}
          onBlur={() => setIsFocused(false)}
        />
        {action}
      </div>
    );
  },
);

TextAreaField.displayName = 'TextAreaField';

export default TextAreaField;
