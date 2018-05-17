using gipo.core;
using gipo.util;
using UnityEngine;

namespace Ark.Core
{
	public interface IBaseSceneLogic : IGearHolder
	{
		void Enter();
		void Exit();
		void Update();
		void SetSceneViewOrder(IBaseSceneViewOrder sceneView);
		void CommandProcess(ICommand command);
	}

	public class BaseSceneLogic<TView> : GearHolder, IBaseSceneLogic where TView : class, IBaseSceneViewOrder
	{
		protected IGameLogic_ForSceneLogic _gameLogic = null;
		protected TView _sceneView = default(TView);

		public BaseSceneLogic() : base(false)
		{
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			_gameLogic = _gear.Absorb<GameLogic>(new PosInfos());

			ArkLog.Debug("BaseSceneLogic Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! シーンに入る時の処理
		public virtual void Enter()
		{
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! シーンから抜け出す時の処理
		public virtual void Exit()
		{
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 更新
		public virtual void Update()
		{

		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndGearProcess()
		{
			base.EndGearProcess();
			ArkLog.Debug("BaseSceneLogic End");
		}
		
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ViewOrderを設定する
		public void SetSceneViewOrder(IBaseSceneViewOrder sceneView)
		{
			_sceneView = (TView)sceneView;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! コマンド処理
		public virtual void CommandProcess(ICommand command)
		{

		}
	}
}