using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam
{
    public class LifeCycle
    {
        private List<IUpdatable> _allUpdatable = new();

        private List<IEntity> _willRemoveList = new();
        private TestBallQue _testQue = new();
        private TestBallPool _testBallPool;

        public void Init(TestBallPool testBallPool)
        {
            _testQue = SingletonContainer.TestBallQue;
            _testBallPool = testBallPool;
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! Entity追加
        protected virtual void AddEntityToAllList(IEntity entity)
        {
            AddToList(_allUpdatable, entity);
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! ライフサイクルにObject追加
        public void AddEntityToLifeCycle(IEntity entity)
        {
            entity.Init();

            AddEntityToAllList(entity);

            entity.Show();
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! Entity削除
        protected virtual void RemoveEntityToAllList(IEntity entity)
        {
            RemoveToList(_allUpdatable, entity);
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! ライフサイクルからObject削除
        public void RemoveEntityToLifeCycle(IEntity entity)
        {
            entity.Hide();
            entity.Release();

            RemoveEntityToAllList(entity);
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! Entity追加 (General)
        protected void AddToList<TData>(List<TData> list, IEntity entity) where TData : class
        {
            if (entity is TData)
            {
                list.Add((TData)entity);
            }
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! Entity削除 (General)
        protected void RemoveToList<TData>(List<TData> list, IEntity entity) where TData : class
        {
            if (entity is TData)
            {
                list.Remove((TData)entity);
            }
        }

        private void UpdateEntity()
        {
            // 毎フレ更新処理
            IUpdatable updatable = null;
            int listLength = _allUpdatable.Count;
            for (int i = 0; i < listLength; ++i)
            {
                updatable = _allUpdatable[i];
                // 処理更新
                updatable.UpdateEntity();
            }
        }

        // -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
        //! 削除
        protected void RemoveEntity()
        {
            int updatableLength = _allUpdatable.Count;
            for (int i = 0; i < updatableLength; ++i)
            {
                if (_allUpdatable[i].IsWillRemove)
                {
                    IEntity removeObj = (IEntity)_allUpdatable[i];
                    _willRemoveList.Add(removeObj);
                }
            }
            int willRemoveListLenght = _willRemoveList.Count;
            for (int i = 0; i < willRemoveListLenght; ++i)
            {
                RemoveEntityToLifeCycle(_willRemoveList[i]);
            }
            if (willRemoveListLenght > 0)
            {
                _willRemoveList.Clear();
            }
        }

        public void UpdateLifeCycle()
        {
            UpdateEntity();

            RemoveEntity();

            ProcessQueue();
        }

        public void ProcessQueue()
        {
            // 生成
            {
                // Effect
                int effectQueLength = _testQue.GetBookLength();
                for (int i = 0; i < effectQueLength; ++i)
                {
                    //var obj = _testQue.Get(i);
                    var obj = _testBallPool.Get();
                    AddEntityToLifeCycle(obj);
                }
                _testQue.ListClear();
            }
        }

        public void Release()
        {

        }
    }
}