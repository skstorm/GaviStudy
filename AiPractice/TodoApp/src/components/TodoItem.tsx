import React from 'react';
import { TodoItem as TodoItemType } from '../models/TodoItem';
import styles from './TodoItem.module.css';

interface TodoItemProps {
  todo: TodoItemType;
  onToggle: (id: string) => void;
  onDelete: (id: string) => void;
}

export const TodoItem: React.FC<TodoItemProps> = ({ todo, onToggle, onDelete }) => {
  const handleToggle = () => {
    onToggle(todo.id);
  };

  const handleDelete = () => {
    onDelete(todo.id);
  };

  return (
    <div className={`${styles.todoItem} ${todo.isCompleted ? styles.completed : ''}`}>
      <label className={styles.checkboxContainer}>
        <input
          type="checkbox"
          checked={todo.isCompleted}
          onChange={handleToggle}
          className={styles.checkbox}
        />
        <span className={styles.checkmark}></span>
      </label>

      <span className={styles.text}>{todo.text}</span>

      <button
        onClick={handleDelete}
        className={styles.deleteButton}
        aria-label="Delete todo"
      >
        ×
      </button>
    </div>
  );
};
