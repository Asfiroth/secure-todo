import { Modal } from '~/components/Modal';
import { type FC, useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import './TodoFormModal.css';
import useTodo from '~/hooks/useTodo.ts';
import toast from 'react-hot-toast';
import { TextField } from '~/components/TextField';
import { SelectField } from '~/components/SelectField';
import { Label } from '~/components/Label';
import { TextAreaField } from '~/components/TextAreaField';
import { TextError } from '~/components/TextError';
import type { Todo } from '~/types/todo.ts';
interface IProps {
  isOpen: boolean;
  setIsOpen: (f: boolean) => void;
  todo?: Todo;
  refresh: () => Promise<void>;
}

interface TodoFormData {
  title: string;
  description?: string;
  priority: string;
  dueDate: string;
}

export const TodoFormModal: FC<IProps> = ({ isOpen, setIsOpen, todo, refresh }) => {
  const [isSubmitted, setIsSubmitted] = useState<boolean>(false);
  const { createTodo, updateTodo } = useTodo();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<TodoFormData>({
    mode: 'onChange',
    reValidateMode: 'onChange',
    defaultValues: {
      title: '',
      priority: 'Low',
      dueDate: new Date().toISOString().split('T')[0],
    },
  });

  useEffect(() => {
    if (!todo) return;
    reset({
      title: todo.title,
      description: todo.description,
      priority: todo.priority,
      dueDate: todo.dueDate,
    });
  }, [todo]);

  const handleCreateTodo = async (formTodo: TodoFormData) => {
    try {
      setIsSubmitted(true);
      const created = await createTodo({
        title: formTodo.title,
        description: formTodo.description,
        priority: formTodo.priority,
        dueDate: formTodo.dueDate,
      });
      if (created) {
        toast.success('Task created successfully');
        reset();
        void refresh();
        setIsOpen(false);
      }
    } catch (e) {
      toast.error('Failed to create task');
      console.error(e);
    } finally {
      setIsSubmitted(false);
    }
  };

  const handleUpdateTodo = async (formTodo: TodoFormData) => {
    try {
      setIsSubmitted(true);
      const updated = await updateTodo({
        id: todo!.id,
        title: formTodo.title,
        description: formTodo.description,
        priority: formTodo.priority,
        dueDate: formTodo.dueDate,
      });
      if (updated) {
        toast.success('Task updated successfully');
        reset();
        void refresh();
        setIsOpen(false);
      }
    } catch (e) {
      toast.error('Failed to update task');
      console.error(e);
    } finally {
      setIsSubmitted(false);
    }
  };

  const onSubmit = async (data: TodoFormData) => {
    if (todo) void handleUpdateTodo(data);
    else void handleCreateTodo(data);
  };

  return (
    <Modal isOpen={isOpen} setIsOpen={setIsOpen} title="Create New Task">
      <form onSubmit={handleSubmit(onSubmit)} className="todo-form">
        <div className="todo-form-body">
          <div className="todo-form-group">
            <Label htmlFor="title">
              Title <span className="required">*</span>
            </Label>
            <TextField
              type="text"
              placeholder="Enter task title"
              {...register('title', {
                required: 'You need to specify a Task title',
                maxLength: 100,
                validate: (value) => value.trim() !== '',
              })}
            />
            {errors.title && <TextError errorMessage={errors.title?.message} />}
          </div>

          <div className="todo-form-group">
            <Label htmlFor="description">Description</Label>
            <TextAreaField
              {...register('description', {
                maxLength: 500,
              })}
              rows={4}
              placeholder="Enter task description (optional)"
            />
            {errors.description && (
              <TextError errorMessage={errors.description?.message} />
            )}
          </div>

          <div className="todo-form-row">
            <div className="todo-form-group">
              <Label htmlFor="priority">
                Priority <span className="required">*</span>
              </Label>
              <SelectField
                options={[
                  { value: 'Low', label: 'Low' },
                  { value: 'Medium', label: 'Medium' },
                  { value: 'High', label: 'High' },
                ]}
                {...register('priority', {
                  required: 'Need to set a Priority for this Task.',
                  validate: (value) => value !== '' || 'Priority cannot be none',
                })}
              />
              {errors.priority && <TextError errorMessage={errors.priority?.message} />}
            </div>

            <div className="todo-form-group">
              <Label htmlFor="dueDate">
                Due Date <span className="required">*</span>
              </Label>
              <TextField
                type="date"
                {...register('dueDate', {
                  required: 'Need to set a Due Date for this Task.',
                  min: {
                    value: new Date().toISOString().split('T')[0],
                    message: 'Due date cannot be in the past',
                  },
                })}
              />
              {errors.dueDate && <TextError errorMessage={errors.dueDate?.message} />}
            </div>
          </div>
        </div>

        <div className="todo-form-footer">
          <button
            type="button"
            className="todo-form-button todo-form-button-secondary"
            disabled={isSubmitted}
            onClick={() => reset()}
          >
            Clear
          </button>
          {!todo && (
            <button
              type="submit"
              className="todo-form-button todo-form-button-primary"
              disabled={isSubmitted}
            >
              {isSubmitted ? 'Creating...' : 'Create Task'}
            </button>
          )}
          {todo && (
            <button
              type="submit"
              className="todo-form-button todo-form-button-primary"
              disabled={isSubmitted}
            >
              {isSubmitted ? 'Updating...' : 'Update Task'}
            </button>
          )}
        </div>
      </form>
    </Modal>
  );
};
