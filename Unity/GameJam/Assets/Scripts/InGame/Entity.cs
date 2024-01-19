using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public abstract class Entity : MonoBehaviour, IEntity, IUpdatable
    {
        // Start is called before the first frame update
        public bool IsActive { get; private set; }

        public Vector2 Pos => Vector2.zero;

        public float Radius => 0;

        public bool IsWillRemove => false;

        protected bool _isUpdateOk = false;

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

        public abstract void UpdateEntity();
    }

}