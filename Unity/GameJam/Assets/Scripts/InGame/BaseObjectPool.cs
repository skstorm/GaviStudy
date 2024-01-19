using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace GameJam
{
    public abstract class BaseObjectPool<TObj> : MonoBehaviour
        where TObj : Entity, IEntity
    {
        private static BaseObjectPool<TObj> s_instance;
        public static BaseObjectPool<TObj> Instance => s_instance;

        [SerializeField]
        private TObj _objOrigin;

        private readonly List<TObj> _objList = new();

        private void Awake()
        {
            s_instance = this;
        }

        public void Init(int createObjNum)
        {
            for (int i = 0; i < createObjNum; ++i)
            {
                createAndAddObj();
            }
        }

        private TObj createAndAddObj()
        {
            var obj = GameObject.Instantiate<TObj>(_objOrigin, transform);
            _objList.Add(obj);
            obj.Hide();
            return obj;
        }

        public TObj Get()
        {
            TObj returnObj = null;
            for (int i = 0; i < _objList.Count; ++i)
            {
                var obj = _objList[i];
                if (obj.IsActive == false)
                {
                    returnObj = obj;
                    break;
                }
            }

            if (returnObj == null)
            {
                returnObj = createAndAddObj();
            }

            returnObj.Show();

            return returnObj;
        }

        public void Release()
        {
            for (int i = 0; i < _objList.Count; ++i)
            {
                Destroy(_objList[i].gameObject);
            }
            _objList.Clear();

            Destroy(gameObject);
        }
    }
}