import { useEffect, useMemo, useState } from 'react';
import { PagedTodoList } from '~/routes/todos/Components';
import { TodoFormModal } from '~/components/TodoFormModal/TodoFormModal.tsx';
import type { Todo, TodoFilter } from '~/types/todo.ts';
import useTodo from '~/hooks/useTodo.ts';
import toast from 'react-hot-toast';
import './View.css';
import { SegmentedControl } from '~/components/SegmentedControl';
import { TextField } from '~/components/TextField';
import { debounce } from 'lodash';

const View = () => {
  const { getMyTodos } = useTodo();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [todos, setTodos] = useState<Todo[]>([]);
  const [isFormOpen, setIsFormOpen] = useState<boolean>(false);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [filter, setFilter] = useState<TodoFilter>({} as TodoFilter);
  const [selectedTodo, setSelectedTodo] = useState<Todo | undefined>();

  const handleUpdate = (todo: Todo) => {
    setSelectedTodo(todo);
    setIsFormOpen(true);
  };

  const loadMyTodos = async () => {
    try {
      if (isLoading) return;
      setIsLoading(true);
      const paged = await getMyTodos({ ...filter, pageSize: 10 });
      setTodos(paged.items);
    } catch (e) {
      console.error(e);
      toast.error('Failed to load todos');
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    void loadMyTodos();
  }, [filter]);

  const debouncedFilter = useMemo(() => {
    return debounce(
      (value: string) => setFilter((prev) => ({ ...prev, searchTerm: value })),
      500,
    );
  }, []);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setSearchTerm(value);
    debouncedFilter(value);
  };

  const onCompletionFilterChange = (filter: string) => {
    setFilter((prev) => ({ ...prev, isCompleted: filter !== 'pending' }));
  };

  useEffect(() => {
    return () => {
      debouncedFilter.cancel();
    };
  }, [debouncedFilter]);

  return (
    <div className="flex flex-1 flex-col items-center">
      <div className="mb-8 flex flex-row items-center gap-4">
        <TextField
          placeholder="Search by title or description..."
          value={searchTerm}
          onChange={handleInputChange}
        />
        <button onClick={() => setIsFormOpen(true)} className="create-todo-btn">
          Add New Task
        </button>
      </div>
      <SegmentedControl
        value={!filter.isCompleted ? 'pending' : 'completed'}
        options={[
          { value: 'pending', label: 'Pending' },
          { value: 'completed', label: 'Completed' },
        ]}
        onChange={onCompletionFilterChange}
      />
      <PagedTodoList
        todos={todos}
        isLoading={isLoading}
        refresh={loadMyTodos}
        handleUpdate={handleUpdate}
      />
      <TodoFormModal
        isOpen={isFormOpen}
        setIsOpen={setIsFormOpen}
        todo={selectedTodo}
        refresh={loadMyTodos}
      />
    </div>
  );
};

export default View;
