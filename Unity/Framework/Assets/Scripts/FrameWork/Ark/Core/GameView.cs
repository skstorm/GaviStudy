using gipo.core;
using gipo.util;
using UnityEngine;

namespace Ark.Core
{
	public interface IGameViewOrder
	{
		IBaseSceneViewOrder SetupSceneView(IBaseSceneLogic sceneLogic);
		IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogic sceneLogic);
	}

	abstract public class GameView : GearHolderBehavior, IGameViewOrder
	{
		private BaseSceneView _currentSceneView = null;
		private ILogicStateChnager_ForView _logicStateChanger = null;

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			_logicStateChanger = _gear.Absorb<LogicStateChanger>(new PosInfos());
			
			ArkLog.Debug("GameView Run");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 描画処理
		public void Render(int deltaFrame)
		{
			_currentSceneView.Render(deltaFrame);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 現在SceneView取得
		public BaseSceneView GetCurrentSceneView()
		{
			return _currentSceneView;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! SceneView作成
		abstract protected BaseSceneView CreateSceneView(IBaseSceneLogic sceneLogic);

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! SceneView設定
		public IBaseSceneViewOrder StartUpSceneView(IBaseSceneLogic sceneLogic)
		{
			// SceneView生成
			_currentSceneView = CreateSceneView(sceneLogic);
			_currentSceneView.InitDI(false);
			// ギアに追加
			_gear.AddChildGear(_currentSceneView.GetGear());

			return _currentSceneView;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! SceneView設定
		public IBaseSceneViewOrder SetupSceneView(IBaseSceneLogic sceneLogic)
		{
			// シーンを親から外す
			_currentSceneView.AllDisposeGear();
			_gear.RemoveChildGear(_currentSceneView.GetGear());
			Destroy(_currentSceneView.gameObject);
			
			// SceneView生成
			_currentSceneView = CreateSceneView(sceneLogic);
			_currentSceneView.InitDI(false);
			// ギアに追加
			_gear.AddChildGear(_currentSceneView.GetGear());

			return _currentSceneView;
		}
	}
}