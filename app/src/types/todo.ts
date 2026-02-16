export type Todo = {
  id: string;
  title: string;
  description?: string;
  priority: string;
  dueDate: Date;
  completed: boolean;
};

export type Paged<T> = {
  items: T[];
  cursor: string;
};
