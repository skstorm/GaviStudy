/*
using Chugen.Battle;

namespace Chugen.Game
{
	public interface ISimpleLifeCycleLogic_ForAlwaysLifeCycleContainer
	{
		EffectQueue EffectQue { get; }
		EtcQueue EtcQue { get; }
		VisualQueue VisualQue { get; }
	}

	public class SimpleLifeCycleLogic : LifeCycleLogic<IBattleLogicForLifeCycle>, ISimpleLifeCycleLogic_ForAlwaysLifeCycleContainer
	{
		private EffectViewPool _effectViewPool = null;
		private EtcObjectViewPool _etcViewPool = null;
		private VisualViewPool _visualViewPool = null;

		private EffectQueue _effectQue = null;
		public EffectQueue EffectQue { get { return _effectQue; } }

		private EtcQueue _etcQue = null;
		public EtcQueue EtcQue { get { return _etcQue; } }

		private VisualQueue _visualQue = null;
		public VisualQueue VisualQue { get { return _visualQue; } }

		public SimpleLifeCycleLogic(IBattleLogicForLifeCycle battleLogic, LifeCycleView lifeCycleView, ELifeCycleKind lifeCycleKind, int randomSeed, float leftWall, float topWall, float rightWall, float bottomWall) 
			: base(battleLogic, lifeCycleView, lifeCycleKind, randomSeed,  leftWall, topWall, rightWall, bottomWall)
		{
			_effectQue = new EffectQueue();
			_effectQue.ListInit();

			_etcQue = new EtcQueue();
			_etcQue.ListInit();

			_visualQue = new VisualQueue();
			_visualQue.ListInit();
		}

		public void Init(EffectViewPool effectViewPool, EtcObjectViewPool etcViewPool, VisualViewPool visualViewPool)
		{
			_effectViewPool = effectViewPool;
			_etcViewPool = etcViewPool;
			_visualViewPool = visualViewPool;
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
				int effectQueLength = _effectQue.GetBookLength();
				for (int i = 0; i < effectQueLength; ++i)
				{
					IBaseEffect obj = _effectQue.Get(i);
					IBattleObjectView view = _effectViewPool.GetView(obj.Kind);
					AddObjectToLifeCycle(obj, view);
				}
				_effectQue.ListClear();
				// etc
				int etcQueLength = _etcQue.GetBookLength();
				for (int i = 0; i < etcQueLength; ++i)
				{
					IEtcObject obj = _etcQue.Get(i);
					IBattleObjectView view = null;
					view = _etcViewPool.GetView(obj.Kind);
					AddObjectToLifeCycle(obj, view);
				}
				_etcQue.ListClear();
				// visual
				int visualQueLength = _visualQue.GetBookLength();
				for (int i = 0; i < visualQueLength; ++i)
				{
					IVisualObject obj = _visualQue.Get(i);
					IBattleObjectView view = _visualViewPool.GetView(obj.Kind, obj.MyCampId, obj.CardOrder);
					AddObjectToLifeCycle(obj, view);
				}
				_visualQue.ListClear();
			}
		}

		public override void Release()
		{
			base.Release();
			// Que整理
			{
				_effectQue.ListRelease();
				_effectQue = null;

				_etcQue.ListRelease();
				_etcQue = null;

				_visualQue.ListRelease();
				_visualQue = null;
			}
			// ViewPool整理
			{
				_effectViewPool = null;
				_etcViewPool = null;
				_visualViewPool = null;
			}
		}
	}
}
*/