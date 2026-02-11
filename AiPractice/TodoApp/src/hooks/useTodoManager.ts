import { useState, useEffect, useCallback } from 'react';
import { TodoItem, createTodoItem } from '../models/TodoItem';
import { storage } from '../utils/storage';

export const useTodoManager = () => {
  const [todos, setTodos] = useState<TodoItem[]>([]);

  useEffect(() => {
    const loadedTodos = storage.loadTodos();
    setTodos(loadedTodos);
    console.log(`Loaded ${loadedTodos.length} todos from storage`);
  }, []);

  useEffect(() => {
    storage.saveTodos(todos);
  }, [todos]);

  const addTodo = useCallback((text: string) => {
    if (!text || text.trim().length === 0) {
      console.warn('Cannot add empty todo item');
      return;
    }

    const newItem = createTodoItem(text);
    setTodos(prev => [...prev, newItem]);
    console.log(`Todo added: ${text}`);
  }, []);

  const deleteTodo = useCallback((id: string) => {
    setTodos(prev => {
      const item = prev.find(t => t.id === id);
      if (!item) {
        console.warn(`Todo with ID ${id} not found`);
        return prev;
      }
      console.log(`Todo deleted: ${item.text}`);
      return prev.filter(t => t.id !== id);
    });
  }, []);

  const toggleTodo = useCallback((id: string) => {
    setTodos(prev => {
      const item = prev.find(t => t.id === id);
      if (!item) {
        console.warn(`Todo with ID ${id} not found`);
        return prev;
      }

      console.log(`Todo toggled: ${item.text}, Completed: ${!item.isCompleted}`);

      return prev.map(t =>
        t.id === id
          ? { ...t, isCompleted: !t.isCompleted }
          : t
      );
    });
  }, []);

  const clearAllTodos = useCallback(() => {
    setTodos([]);
    storage.clearTodos();
    console.log('All todos cleared');
  }, []);

  return {
    todos,
    addTodo,
    deleteTodo,
    toggleTodo,
    clearAllTodos,
  };
};
