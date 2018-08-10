using Ark.LifeCycle;

namespace Example
{
	public interface ISimpleLifeCycleLogic_ForAlwaysLifeCycleContainer
	{
		UnitQueue UnitQue { get; }
	}

	public class SimpleLifeCycleLogic : LifeCycleLogic<IEntityFieldLogic_ForLifeCycle>, ISimpleLifeCycleLogic_ForAlwaysLifeCycleContainer
	{
		private UnitViewPool _unitViewPool = null;

		private UnitQueue _unitQue = null;
		public UnitQueue UnitQue { get { return _unitQue; } }

		public SimpleLifeCycleLogic(IEntityFieldLogic_ForLifeCycle entityLogic, LifeCycleView lifeCycleView, ELifeCycleKind lifeCycleKind, int randomSeed, float leftWall, float topWall, float rightWall, float bottomWall) 
			: base(entityLogic, lifeCycleView, lifeCycleKind, randomSeed,  leftWall, topWall, rightWall, bottomWall)
		{
			_unitQue = new UnitQueue();
			_unitQue.ListInit();
		}

		public void Init(UnitViewPool effectViewPool)
		{
			_unitViewPool = effectViewPool;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ライフサイクルを更新する
		public override void UpdateLifeCycle()
		{
			// 移動
			Move();
			// 壁の中に閉じ込める
			ConfineInWall();
			// 更新
			Update();
			// 削除
			Remove();
			// Queを処理する
			ProcessQueue();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Queを処理する
		public override void ProcessQueue()
		{
			// 生成
			{
				// Effect
				int effectQueLength = _unitQue.GetBookLength();
				for (int i = 0; i < effectQueLength; ++i)
				{
					ITestUnitLogic obj = _unitQue.Get(i);
					IEntityView view = _unitViewPool.GetView(obj.Kind);
					AddEntityToLifeCycle(obj, view);
				}
				_unitQue.ListClear();
			}
		}

		public override void Release()
		{
			base.Release();
			// Que整理
			{
				_unitQue.ListRelease();
				_unitQue = null;
			}
			// ViewPool整理
			{
				_unitViewPool = null;
			}
		}
	}
}