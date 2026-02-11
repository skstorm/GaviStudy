using System;
using System.Collections.Generic;
using UnityEngine;

namespace GaviPractice.TodoApp
{
    public class TodoManager : MonoBehaviour
    {
        // Singleton pattern
        private static TodoManager _instance;
        public static TodoManager Instance => _instance;

        // Todo items collection
        private List<TodoItem> _todoItems;
        public List<TodoItem> TodoItems => _todoItems;

        // Events for UI updates
        public event Action<TodoItem> OnTodoAdded;
        public event Action<string> OnTodoDeleted;
        public event Action<TodoItem> OnTodoToggled;

        private void Awake()
        {
            // Singleton initialization
            if (_instance == null)
            {
                _instance = this;
                _todoItems = new List<TodoItem>();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Add new todo item
        /// </summary>
        public void AddTodo(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                Debug.LogWarning("Cannot add empty todo item");
                return;
            }

            TodoItem newItem = new TodoItem(text);
            _todoItems.Add(newItem);
            OnTodoAdded?.Invoke(newItem);

            Debug.Log($"Todo added: {text}");
        }

        /// <summary>
        /// Delete todo item by ID
        /// </summary>
        public void DeleteTodo(string id)
        {
            TodoItem item = _todoItems.Find(x => x.Id == id);
            if (item != null)
            {
                _todoItems.Remove(item);
                OnTodoDeleted?.Invoke(id);
                Debug.Log($"Todo deleted: {item.Text}");
            }
            else
            {
                Debug.LogWarning($"Todo with ID {id} not found");
            }
        }

        /// <summary>
        /// Toggle todo completion status
        /// </summary>
        public void ToggleTodo(string id)
        {
            TodoItem item = _todoItems.Find(x => x.Id == id);
            if (item != null)
            {
                item.IsCompleted = !item.IsCompleted;
                OnTodoToggled?.Invoke(item);
                Debug.Log($"Todo toggled: {item.Text}, Completed: {item.IsCompleted}");
            }
            else
            {
                Debug.LogWarning($"Todo with ID {id} not found");
            }
        }

        /// <summary>
        /// Get todo by ID
        /// </summary>
        public TodoItem GetTodo(string id)
        {
            return _todoItems.Find(x => x.Id == id);
        }

        /// <summary>
        /// Get all todos
        /// </summary>
        public List<TodoItem> GetAllTodos()
        {
            return new List<TodoItem>(_todoItems);
        }

        /// <summary>
        /// Clear all todos
        /// </summary>
        public void ClearAllTodos()
        {
            _todoItems.Clear();
            Debug.Log("All todos cleared");
        }
    }
}
