import apiClient from '~/lib/api.ts';
import type { Paged, Todo } from '~/types/todo.ts';

const useTodo = () => {
  const getMyTodos = async () => {
    const { data } = await apiClient.get<Paged<Todo>>('/tasks');
    console.info(data.cursor);
  };

  const createTodo = () => {};

  const completeTodo = () => {};

  const deleteTodo = () => {};

  return {
    getMyTodos,
    createTodo,
    completeTodo,
    deleteTodo,
  };
};

export default useTodo;
