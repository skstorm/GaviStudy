using System;
using UnityEngine;

namespace GaviPractice.TodoApp
{
    [Serializable]
    public class TodoItem
    {
        [SerializeField] private string _id;
        [SerializeField] private string _text;
        [SerializeField] private bool _isCompleted;
        [SerializeField] private DateTime _createdAt;

        public string Id => _id;
        public string Text
        {
            get => _text;
            set => _text = value;
        }
        public bool IsCompleted
        {
            get => _isCompleted;
            set => _isCompleted = value;
        }
        public DateTime CreatedAt => _createdAt;

        public TodoItem(string text)
        {
            _id = Guid.NewGuid().ToString();
            _text = text;
            _isCompleted = false;
            _createdAt = DateTime.Now;
        }
    }
}
