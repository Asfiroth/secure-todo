import apiClient from '~/lib/api.ts';
import type { Paged, Todo, TodoFilter } from '~/types/todo.ts';
import { queryParamAssembler } from '~/utils/http.helpers.ts';

const useTodo = () => {
  const getMyTodos = async ({
    searchTerm,
    isCompleted = false,
    pageSize = 10,
    cursor,
  }: TodoFilter): Promise<Paged<Todo>> => {
    const queryParams = queryParamAssembler({
      searchTerm,
      isCompleted,
      pageSize,
      cursor,
    });

    const { data } = await apiClient.get<Paged<Todo>>(`/tasks${queryParams}`);
    return data;
  };

  const createTodo = async (todoData: Partial<Todo>): Promise<boolean> => {
    const { status } = await apiClient.post<Todo>('/tasks', todoData);
    return status === 201;
  };

  const completeTodo = async (id: string): Promise<boolean> => {
    const { status } = await apiClient.post(`/tasks/${id}/completion`);
    return status === 204;
  };

  const incompleteTodo = async (id: string): Promise<boolean> => {
    const { status } = await apiClient.delete(`/tasks/${id}/completion`);
    return status === 200;
  };

  const updateTodo = async (todoData: Partial<Todo>): Promise<boolean> => {
    const { status } = await apiClient.put(`/tasks/${todoData.id}`, todoData);
    return status === 204;
  };

  const deleteTodo = async (id: string): Promise<boolean> => {
    const { status } = await apiClient.delete(`/tasks/${id}`);
    return status === 200;
  };

  return {
    getMyTodos,
    createTodo,
    updateTodo,
    completeTodo,
    incompleteTodo,
    deleteTodo,
  };
};

export default useTodo;
