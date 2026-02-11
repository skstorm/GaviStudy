using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GaviPractice.TodoApp
{
    public class TodoItemUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Toggle _completionToggle;
        [SerializeField] private TextMeshProUGUI _todoText;
        [SerializeField] private Button _deleteButton;
        [SerializeField] private Image _backgroundImage;

        [Header("Visual Settings")]
        [SerializeField] private Color _normalColor = Color.white;
        [SerializeField] private Color _completedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        [SerializeField] private Color _strikethroughColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        private string _todoId;
        private bool _isInitialized = false;

        /// <summary>
        /// Initialize the UI with todo data
        /// </summary>
        public void Initialize(TodoItem item)
        {
            _todoId = item.Id;
            _todoText.text = item.Text;
            _completionToggle.isOn = item.IsCompleted;

            // Setup listeners
            _completionToggle.onValueChanged.AddListener(OnToggleChanged);
            _deleteButton.onClick.AddListener(OnDeleteButtonClicked);

            // Update visual state
            UpdateCompletionState(item.IsCompleted);

            _isInitialized = true;
        }

        private void OnDestroy()
        {
            // Remove listeners
            if (_isInitialized)
            {
                _completionToggle.onValueChanged.RemoveListener(OnToggleChanged);
                _deleteButton.onClick.RemoveListener(OnDeleteButtonClicked);
            }
        }

        /// <summary>
        /// Handle toggle value change
        /// </summary>
        private void OnToggleChanged(bool isOn)
        {
            TodoManager.Instance.ToggleTodo(_todoId);
        }

        /// <summary>
        /// Handle delete button click
        /// </summary>
        private void OnDeleteButtonClicked()
        {
            TodoManager.Instance.DeleteTodo(_todoId);
        }

        /// <summary>
        /// Update visual state based on completion
        /// </summary>
        public void UpdateCompletionState(bool isCompleted)
        {
            // Update background color
            _backgroundImage.color = isCompleted ? _completedColor : _normalColor;

            // Update text style (strikethrough for completed)
            if (isCompleted)
            {
                _todoText.fontStyle = FontStyles.Strikethrough;
                _todoText.color = _strikethroughColor;
            }
            else
            {
                _todoText.fontStyle = FontStyles.Normal;
                _todoText.color = Color.black;
            }
        }
    }
}
