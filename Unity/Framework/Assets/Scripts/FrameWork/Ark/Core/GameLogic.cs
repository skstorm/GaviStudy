using Ark.Gear;
using Ark.Util;

namespace Ark.Core
{
	public interface IGameLogic_ForLogicStateChanger
	{
		void NotifyCommand(ICommand command);
	}

	public interface IGameLogic_ForSceneLogic
	{
		void ChangeScene(IBaseSceneLogic nextScene);
	}

	public class GameLogic : GearHolder, IGameLogic_ForLogicStateChanger, IGameLogic_ForSceneLogic
	{
		// GameView
		private IGameViewOrder _gameView = null;
		// 現在シーンのLogic
		private IBaseSceneLogic _currentSceneLogic = null;
		// 前のシーンLogic
		private IBaseSceneLogic _prevSceneLogic = null;

		public GameLogic(ISetting setting) : base(false)
		{
			// 開始シーン設定
			_currentSceneLogic = setting.StartScene;
			_gear.AddChildGear(_currentSceneLogic.GetGear());
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			_gameView = _gear.Absorb<GameView>(new PosInfos());
			IBaseSceneViewOrder sceneView = _gameView.StartUpSceneView(_currentSceneLogic);
			_currentSceneLogic.SetSceneViewOrder(sceneView);

			ArkLog.Debug("Game Logic Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndGearProcess()
		{
			base.EndGearProcess();
			ArkLog.Debug("Game Logic End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 更新
		public void Update()
		{
			_currentSceneLogic.Update();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! シーン変更
		public void ChangeScene(IBaseSceneLogic nextScene)
		{
			// 現在のSceneLogicが抜ける時の処理
			_currentSceneLogic.Exit();
			// ギアを解除
			_currentSceneLogic.AllDisposeGear();
			// 現在のSceneLogicを前のSceneLogicに格納する
			_prevSceneLogic = _currentSceneLogic;
			// ギアの親子関係から外す
			_gear.RemoveChildGear(_prevSceneLogic.GetGear());
			// 現在のSceneLogicを新しいものに入れ替える
			_currentSceneLogic = nextScene;
			// SceneView作成
			IBaseSceneViewOrder sceneView = _gameView.SetupSceneView(_currentSceneLogic);
			// SceneViewを設定
			_currentSceneLogic.SetSceneViewOrder(sceneView);
			// 新しいSceneLogicを子供として追加
			_gear.AddChildGear(_currentSceneLogic.GetGear());
			// 現在のSceneLogicのギアの初期化
			_currentSceneLogic.InitGear();
			// 現在のSceneViewのギアの初期化
			sceneView.InitGear();
			// 現在のSceneLogicに入る時の処理
			_currentSceneLogic.Enter();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! コマンド通達
		public void NotifyCommand(ICommand command)
		{
			_currentSceneLogic.CommandProcess(command);
		}
	}
}