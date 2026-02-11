import React from 'react';
import { TodoItem as TodoItemType } from '../models/TodoItem';
import { TodoItem } from './TodoItem';
import styles from './TodoList.module.css';

interface TodoListProps {
  todos: TodoItemType[];
  onToggleTodo: (id: string) => void;
  onDeleteTodo: (id: string) => void;
}

export const TodoList: React.FC<TodoListProps> = ({
  todos,
  onToggleTodo,
  onDeleteTodo
}) => {
  if (todos.length === 0) {
    return (
      <div className={styles.empty}>
        <p className={styles.emptyText}>No todos yet!</p>
        <p className={styles.emptySubtext}>Add a task above to get started.</p>
      </div>
    );
  }

  return (
    <div className={styles.todoList}>
      {todos.map(todo => (
        <TodoItem
          key={todo.id}
          todo={todo}
          onToggle={onToggleTodo}
          onDelete={onDeleteTodo}
        />
      ))}
    </div>
  );
};
