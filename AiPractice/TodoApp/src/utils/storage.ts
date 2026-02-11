import { TodoItem, SerializedTodoItem } from '../models/TodoItem';

const STORAGE_KEY = 'todoapp_items';

export const storage = {
  loadTodos: (): TodoItem[] => {
    try {
      const stored = localStorage.getItem(STORAGE_KEY);
      if (!stored) return [];

      const serialized: SerializedTodoItem[] = JSON.parse(stored);
      return serialized.map(item => ({
        ...item,
        createdAt: new Date(item.createdAt),
      }));
    } catch (error) {
      console.error('Failed to load todos from localStorage:', error);
      return [];
    }
  },

  saveTodos: (todos: TodoItem[]): void => {
    try {
      const serialized: SerializedTodoItem[] = todos.map(item => ({
        ...item,
        createdAt: item.createdAt.toISOString(),
      }));

      localStorage.setItem(STORAGE_KEY, JSON.stringify(serialized));
    } catch (error) {
      console.error('Failed to save todos to localStorage:', error);
    }
  },

  clearTodos: (): void => {
    try {
      localStorage.removeItem(STORAGE_KEY);
    } catch (error) {
      console.error('Failed to clear todos from localStorage:', error);
    }
  },
};
