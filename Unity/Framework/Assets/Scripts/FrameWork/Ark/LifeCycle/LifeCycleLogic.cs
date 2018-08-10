using System.Collections.Generic;

namespace Ark.LifeCycle
{
	public enum ELifeCycleKind
	{
		SyncBattle, // 同期すべきライフサイクル
		LocalBattle, // 自分だけ存在するObjectが動くライフサイクル
		AlwaysBattle, // 常に回るライフサイクル
	}

	public interface ILifeCycleLogic
	{
		ELifeCycleKind Kind { get; }
	}

	abstract public class LifeCycleLogic<TEntityFieldLogic> : ILifeCycleLogic where TEntityFieldLogic : IEntityFieldLogic_ForLifeCycle
	{
		protected TEntityFieldLogic _entityFieldLogic = default(TEntityFieldLogic);
		// 各アクション別のInterfaceのList
		protected List<IMovable> _allMovable = new List<IMovable>();
		protected List<IUpdatable> _allUpdatable = new List<IUpdatable>();
		protected List<IEntityLogic> _willRemoveList = new List<IEntityLogic>();
		// ライフサイクルのView
		protected LifeCycleView _lifeCycleView = null;
		// 壁
		protected readonly float _leftWall = 0;
		protected readonly float _topWall = 0;
		protected readonly float _rightWall = 0;
		protected readonly float _bottomWall = 0;
		// 一定間隔で行う更新処理 
		private int _periodicUpdateCount = 0;
		// ライフサイクルの種類
		protected readonly ELifeCycleKind _kind;
		public ELifeCycleKind Kind { get { return _kind; } }
		// ランダムクラス
		protected readonly System.Random _random;
		public System.Random Random { get { return _random; } }

		protected LifeCycleLogic(TEntityFieldLogic entityFieldLogic, LifeCycleView lifeCycleView, ELifeCycleKind lifeCycleKind, int randomSeed)
		{
			_entityFieldLogic = entityFieldLogic;
			_lifeCycleView = lifeCycleView;
			_kind = lifeCycleKind;
			_random = new System.Random(randomSeed);
		}

		protected LifeCycleLogic(TEntityFieldLogic entityLogic, LifeCycleView lifeCycleView, ELifeCycleKind lifeCycleKind, int randomSeed, float leftWall, float topWall, float rightWall, float bottomWall) 
		: this(entityLogic, lifeCycleView, lifeCycleKind, randomSeed)
		{
			_leftWall = leftWall;
			_topWall = topWall;
			_rightWall = rightWall;
			_bottomWall = bottomWall;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ライフサイクルを更新する
		abstract public void UpdateLifeCycle();

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Queを処理する
		abstract public void ProcessQueue();

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 移動
		protected void Move()
		{
			int listLength = _allMovable.Count;
			for (int i = 0; i < listLength; ++i)
			{
				_allMovable[i].Move();
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Objectを指定範囲内に収める
		protected void ConfineInWall()
		{
			IMovable movable = null;
			int listLength = _allMovable.Count;
			for (int i = 0; i < listLength; ++i)
			{
				movable = _allMovable[i];
				if (!movable.IsOutsideMap && movable.IsCheckWall)
				{
					movable.ConfineInWall(_leftWall, _topWall, _rightWall, _bottomWall);
				}
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 一定間隔の更新処理
		protected void PeriodicUpdate(int preriodicUpdateIntervalTime)
		{
			// 一定間隔の更新処理
			++_periodicUpdateCount;
			bool isPeriodicUpdate = _periodicUpdateCount > preriodicUpdateIntervalTime;
			if (isPeriodicUpdate)
			{
				int objLength = _allUpdatable.Count;
				for (int i = 0; i < objLength; ++i)
				{
					_allUpdatable[i].PeriodicUpdate();
				}
				_periodicUpdateCount = 0;
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 更新
		protected void Update()
		{
			// 毎フレ更新処理
			IUpdatable updatable = null;
			int listLength = _allUpdatable.Count;
			for (int i = 0; i < listLength; ++i)
			{
				updatable = _allUpdatable[i];
				// 状態更新
				updatable.UpdateChangeState();
				// 処理更新
				updatable.Update();
				// viewを変更する
				if (updatable.IsWillChangeView)
				{
					IEntityLogic obj = (IEntityLogic)updatable;
					IEntityView changeView = (IEntityView)updatable.GetWillChangeView();
					_lifeCycleView.RemoveEntity(obj);
					updatable.SetView(changeView);
					_lifeCycleView.AddEntity(obj, changeView);
					updatable.EndChangeView();
				}
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 削除
		protected void Remove()
		{
			int updatableLength = _allUpdatable.Count;
			for (int i = 0; i < updatableLength; ++i)
			{
				if (_allUpdatable[i].IsWillRemove)
				{
					IEntityLogic removeObj = (IEntityLogic)_allUpdatable[i];
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

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Entity追加
		protected virtual void AddEntityToAllList(IEntityLogic entity)
		{
			AddToList(_allMovable, entity);
			AddToList(_allUpdatable, entity);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ライフサイクルにObject追加
		public void AddEntityToLifeCycle(IEntityLogic logic, IEntityView view)
		{
			logic.Init(_entityFieldLogic, view, _kind);
			
			AddEntityToAllList(logic);

			_lifeCycleView.AddEntity(logic, view);
			logic.Start();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Entity削除
		protected virtual void RemoveEntityToAllList(IEntityLogic entity)
		{
			RemoveToList(_allMovable, entity);
			RemoveToList(_allUpdatable, entity);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ライフサイクルからObject削除
		public void RemoveEntityToLifeCycle(IEntityLogic entity)
		{
			entity.Release();

			RemoveEntityToAllList(entity);

			_lifeCycleView.RemoveEntity(entity);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Entity追加 (General)
		protected void AddToList<TData>(List<TData> list, IEntityLogicPeek entity) where TData : class
		{
			if (entity is TData)
			{
				list.Add((TData)entity);
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Entity削除 (General)
		protected void RemoveToList<TData>(List<TData> list, IEntityLogicPeek entity) where TData : class
		{
			if (entity is TData)
			{
				list.Remove((TData)entity);
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 
		public virtual void Release()
		{
			_allMovable = null;
			_allUpdatable = null;
			_willRemoveList = null;

			_lifeCycleView = null;
			_entityFieldLogic = default(TEntityFieldLogic);
		}
	}
}