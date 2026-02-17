import './SegmentedControl.css';

interface SegmentedControlProps<T extends string = string> {
  value?: T;
  onChange?: (value: T) => void;
  options: { value: T; label: string }[];
}

export function SegmentedControl<T extends string = string>({
  value,
  onChange,
  options,
}: SegmentedControlProps<T>) {
  return (
    <div className="segmented-control">
      {options.map((opt) => {
        const selected = value === opt.value;
        return (
          <div
            key={opt.value}
            onClick={() => onChange?.(opt.value)}
            className={`segment ${selected ? 'selected' : ''} `}
          >
            {opt.label}
          </div>
        );
      })}
    </div>
  );
}
