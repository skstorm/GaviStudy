import React from 'react';
import { useTodoManager } from './hooks/useTodoManager';
import { TodoInput } from './components/TodoInput';
import { TodoList } from './components/TodoList';
import styles from './App.module.css';

function App() {
  const { todos, addTodo, deleteTodo, toggleTodo, clearAllTodos } = useTodoManager();

  const totalTodos = todos.length;
  const completedTodos = todos.filter(t => t.isCompleted).length;
  const activeTodos = totalTodos - completedTodos;

  return (
    <div className={styles.app}>
      <div className={styles.container}>
        <header className={styles.header}>
          <h1 className={styles.title}>Todo App</h1>
          <p className={styles.subtitle}>Keep track of your tasks</p>
        </header>

        <div className={styles.stats}>
          <div className={styles.stat}>
            <span className={styles.statValue}>{totalTodos}</span>
            <span className={styles.statLabel}>Total</span>
          </div>
          <div className={styles.stat}>
            <span className={styles.statValue}>{activeTodos}</span>
            <span className={styles.statLabel}>Active</span>
          </div>
          <div className={styles.stat}>
            <span className={styles.statValue}>{completedTodos}</span>
            <span className={styles.statLabel}>Completed</span>
          </div>
        </div>

        <TodoInput onAddTodo={addTodo} />

        <TodoList
          todos={todos}
          onToggleTodo={toggleTodo}
          onDeleteTodo={deleteTodo}
        />

        {totalTodos > 0 && (
          <button
            onClick={clearAllTodos}
            className={styles.clearButton}
          >
            Clear All Todos
          </button>
        )}
      </div>
    </div>
  );
}

export default App;
