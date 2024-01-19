using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class PoolObject : MonoBehaviour, IEntity
    {
        public bool IsActive { get; private set; }

        protected bool _isUpdateOk = false;

        public Vector2 Pos => Vector2.zero;

        public float Radius => 0;

        public bool IsWillRemove => false;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            IsActive = true;
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
            IsActive = false;
            _isUpdateOk = false;
        }

        public virtual void Init()
        {
            _isUpdateOk = true;
        }

        public virtual void Release()
        {
            _isUpdateOk = false;
        }
    }
}