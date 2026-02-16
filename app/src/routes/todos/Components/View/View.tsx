import useTodo from '~/hooks/useTodo.ts';
import { useEffect, useState } from 'react';
import { PagedTodoList, TodoForm } from '~/routes/todos/Components';

const View = () => {
  const { getMyTodos } = useTodo();
  const [isLoading, setIsLoading] = useState<boolean>(false);

  useEffect(() => {
    const loadMyTodos = async () => {
      try {
        if (isLoading) return;
        setIsLoading(true);
        void getMyTodos();
      } catch (e) {
        console.error(e);
      } finally {
        setIsLoading(false);
      }
    };
    void loadMyTodos();
  }, [getMyTodos, isLoading]);

  return (
    <div className="flex flex-1 flex-col">
      <TodoForm />
      <PagedTodoList />
    </div>
  );
};

export default View;
