export type Todo = {
  id: string;
  title: string;
  description?: string;
  priority: string;
  dueDate: string;
  isCompleted: boolean;
};

export type Paged<T> = {
  items: T[];
  cursor: string;
};


export interface TodoFilter {
  searchTerm?: string;
  isCompleted: boolean;
  pageSize: number;
  cursor?: string;
};