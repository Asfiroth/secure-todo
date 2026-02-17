import React, { useState } from 'react';
import { ChevronDownIcon } from '@heroicons/react/20/solid';

import './SelectField.css';
export interface SelectOption {
  value: string | number;
  label: string;
  isPlaceholder?: boolean;
}

interface SelectFieldProps extends React.SelectHTMLAttributes<HTMLSelectElement> {
  options?: SelectOption[];
}

export const SelectField: React.FC<SelectFieldProps> = ({ options, children, ...props }) => {
  const [isFocused, setIsFocused] = useState<boolean>(false);
  return (
    <div className={`select-field relative ${isFocused ? 'focus' : ''}`}>
      <select
        {...props}
        className={`select-field-input w-full appearance-none pr-10`}
        onFocus={() => setIsFocused(true)}
        onBlur={() => setIsFocused(false)}
      >
        {children ??
          options?.map(({ value, label, isPlaceholder }) => (
            <option key={value} value={value} disabled={isPlaceholder}>
              {label}
            </option>
          ))}
      </select>
      <div className="chevron-icon">
        <ChevronDownIcon className="my-auto h-4 w-4" />
      </div>
    </div>
  );
};
