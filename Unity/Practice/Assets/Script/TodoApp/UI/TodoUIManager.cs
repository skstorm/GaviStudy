using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace GaviPractice.TodoApp
{
    public class TodoUIManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _addButton;
        [SerializeField] private Transform _todoListContainer;
        [SerializeField] private GameObject _todoItemPrefab;
        [SerializeField] private Button _clearAllButton;

        [Header("Empty State")]
        [SerializeField] private GameObject _emptyStatePanel;
        [SerializeField] private TextMeshProUGUI _emptyStateText;

        // Track instantiated UI items
        private Dictionary<string, TodoItemUI> _todoUIItems;

        private void Awake()
        {
            _todoUIItems = new Dictionary<string, TodoItemUI>();
        }

        private void Start()
        {
            // Setup button listeners
            _addButton.onClick.AddListener(OnAddButtonClicked);
            _clearAllButton.onClick.AddListener(OnClearAllButtonClicked);

            // Setup input field listener
            _inputField.onSubmit.AddListener(OnInputFieldSubmit);

            // Subscribe to TodoManager events
            TodoManager.Instance.OnTodoAdded += OnTodoAdded;
            TodoManager.Instance.OnTodoDeleted += OnTodoDeleted;
            TodoManager.Instance.OnTodoToggled += OnTodoToggled;

            // Show empty state initially
            UpdateEmptyState();
        }

        private void OnDestroy()
        {
            // Unsubscribe from events
            if (TodoManager.Instance != null)
            {
                TodoManager.Instance.OnTodoAdded -= OnTodoAdded;
                TodoManager.Instance.OnTodoDeleted -= OnTodoDeleted;
                TodoManager.Instance.OnTodoToggled -= OnTodoToggled;
            }

            // Remove button listeners
            _addButton.onClick.RemoveListener(OnAddButtonClicked);
            _clearAllButton.onClick.RemoveListener(OnClearAllButtonClicked);
            _inputField.onSubmit.RemoveListener(OnInputFieldSubmit);
        }

        /// <summary>
        /// Handle add button click
        /// </summary>
        private void OnAddButtonClicked()
        {
            AddTodoFromInput();
        }

        /// <summary>
        /// Handle input field submit (Enter key)
        /// </summary>
        private void OnInputFieldSubmit(string text)
        {
            AddTodoFromInput();
        }

        /// <summary>
        /// Add todo from input field
        /// </summary>
        private void AddTodoFromInput()
        {
            string text = _inputField.text;
            if (!string.IsNullOrWhiteSpace(text))
            {
                TodoManager.Instance.AddTodo(text);
                _inputField.text = string.Empty;
                _inputField.ActivateInputField();
            }
        }

        /// <summary>
        /// Handle clear all button click
        /// </summary>
        private void OnClearAllButtonClicked()
        {
            // Clear all UI items
            foreach (var kvp in _todoUIItems)
            {
                Destroy(kvp.Value.gameObject);
            }
            _todoUIItems.Clear();

            // Clear data
            TodoManager.Instance.ClearAllTodos();
            UpdateEmptyState();
        }

        /// <summary>
        /// Event handler: Todo added
        /// </summary>
        private void OnTodoAdded(TodoItem item)
        {
            // Instantiate prefab
            GameObject itemObj = Instantiate(_todoItemPrefab, _todoListContainer);
            TodoItemUI itemUI = itemObj.GetComponent<TodoItemUI>();

            // Initialize UI
            itemUI.Initialize(item);

            // Track in dictionary
            _todoUIItems.Add(item.Id, itemUI);

            UpdateEmptyState();
        }

        /// <summary>
        /// Event handler: Todo deleted
        /// </summary>
        private void OnTodoDeleted(string id)
        {
            if (_todoUIItems.TryGetValue(id, out TodoItemUI itemUI))
            {
                Destroy(itemUI.gameObject);
                _todoUIItems.Remove(id);
                UpdateEmptyState();
            }
        }

        /// <summary>
        /// Event handler: Todo toggled
        /// </summary>
        private void OnTodoToggled(TodoItem item)
        {
            if (_todoUIItems.TryGetValue(item.Id, out TodoItemUI itemUI))
            {
                itemUI.UpdateCompletionState(item.IsCompleted);
            }
        }

        /// <summary>
        /// Update empty state visibility
        /// </summary>
        private void UpdateEmptyState()
        {
            bool isEmpty = _todoUIItems.Count == 0;
            _emptyStatePanel.SetActive(isEmpty);
        }
    }
}
