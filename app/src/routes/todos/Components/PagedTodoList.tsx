import type { FC } from 'react';
import type { Todo } from '~/types/todo.ts';
import { Loader } from '~/components/Loader';
import { CheckIcon, PencilIcon, TrashIcon, XMarkIcon } from '@heroicons/react/20/solid';
import useTodo from '~/hooks/useTodo.ts';
import { Card } from '~/components/Card';
import toast from 'react-hot-toast';

interface IProp {
  todos: Todo[];
  isLoading: boolean;
  refresh: () => Promise<void>;
  handleUpdate: (todo: Todo) => void;
}

export const PagedTodoList: FC<IProp> = ({ isLoading, todos, refresh, handleUpdate }) => {
  const { deleteTodo, completeTodo, incompleteTodo } = useTodo();

  const handleDelete = async (id: string) => {
    try {
      const result = await deleteTodo(id);
      if (result) {
        toast.success('Todo deleted successfully');
        await refresh();
      }
    } catch (e) {
      toast.error('Failed to delete todo');
      console.error(e);
    }
  };

  const handleCompletion = async (id: string) => {
    try {
      const result = await completeTodo(id);
      if (result) {
        toast.success('Todo marked as complete');
        await refresh();
      }
    } catch (e) {
      toast.error('Failed to mark todo as complete');
      console.error(e);
    }
  };

  const handleIncompletion = async (id: string) => {
    try {
      const result = await incompleteTodo(id);
      if (result) {
        toast.success('Todo marked as incomplete');
        await refresh();
      }
    } catch (e) {
      toast.error('Failed to mark todo as incomplete');
      console.error(e);
    }
  };

  if (isLoading) return <Loader />;
  return todos.map((todo) => (
    <Card key={todo.id}>
      <div className="title">{todo.title}</div>
      <div className="flex flex-row">
        <div className="content todo-content">
          <div className="description">{todo.description}</div>
          <div className="due-date">{todo.dueDate}</div>
          <div className={`priority-${todo.priority.toLowerCase()}`}>{todo.priority}</div>
        </div>
        {!todo.isCompleted && (
          <div className="actions">
            <button className="delete-btn" onClick={() => handleDelete(todo.id)}>
              <TrashIcon className="h-4 w-4" />
            </button>
            <button className="edit-btn" onClick={() => handleUpdate(todo)}>
              <PencilIcon className="h-4 w-4" />
            </button>
            <button className="complete-btn" onClick={() => handleCompletion(todo.id)}>
              <CheckIcon className="h-4 w-4" />
            </button>
          </div>
        )}
        {todo.isCompleted && (
          <div className="actions">
            <button className="edit-btn" onClick={() => handleIncompletion(todo.id)}>
              <XMarkIcon className="h-4 w-4" />
            </button>
          </div>
        )}
      </div>
    </Card>
  ));
};
