using UnityEngine;
using gipo.core;
using gipo.util;

namespace Core
{
	public class GameLoop : GearHolder
	{
		private FrameManager _frameManager = null;
		private GameLogic _gameLogic = null;
		private GameView _gameView = null;
		private LogicStateChanger _logicStateChanger = null;
		private CommandRecorder _commandRecorder = null;
		private CommandReplayer _commandReplayer = null;

		private float _debugLastUpdateSeconds = 0;
		private float _debugDeltaFrame = 0;
		private int _debugCount = 0;

		public GameLoop(ISetting setting, GameView gameView) : base(true)
		{
			_frameManager = new FrameManager();
			_gameLogic = new GameLogic(setting);
			_gameView = gameView;
			_gameView.InitDI(false);
			_logicStateChanger = new LogicStateChanger();
			_commandRecorder = new CommandRecorder();
			_commandReplayer = new CommandReplayer();

			_gear.AddChildGear(_frameManager.GetGear());
			_gear.AddChildGear(_gameLogic.GetGear());
			_gear.AddChildGear(_gameView.GetGear());
			_gear.AddChildGear(_logicStateChanger.GetGear());
			_gear.AddChildGear(_commandRecorder.GetGear());
			_gear.AddChildGear(_commandReplayer.GetGear());
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 子供にギアを拡散する
		protected override void GearDiffuse()
		{
			base.GearDiffuse();
			
			_gear.Diffuse(_frameManager, typeof(FrameManager));
			_gear.Diffuse(_gameLogic, typeof(GameLogic));
			_gear.Diffuse(_gameView, typeof(GameView));
			_gear.Diffuse(_logicStateChanger, typeof(LogicStateChanger));
			_gear.Diffuse(_commandRecorder, typeof(CommandRecorder));
			_gear.Diffuse(_commandReplayer, typeof(CommandReplayer));
		}


		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void Run()
		{
			base.Run();
			Debug.Log("Game Loop Run");

			// FrameManagerの時間初期化
			_frameManager.RecordLastUpdateSeconds();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void DisposeProcess()
		{
			base.DisposeProcess();
			Debug.Log("Game Loop DisposeProcess");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 更新
		public void Update()
		{
			int deltaFrame = _frameManager.CalcDeltaFrame();
			_frameManager.RecordLastUpdateSeconds();
			
			//////////////////// debug ////////////////
			_debugCount += deltaFrame;
			_debugDeltaFrame += (Time.time - _debugLastUpdateSeconds);
			if(_debugDeltaFrame >= 1.0f)
			{
				//Debug.Log("count : "+ _debugCount);
				_debugDeltaFrame = 0.0f;
				_debugCount = 0;
			}
			_debugLastUpdateSeconds = Time.time;
			//////////////////// debug ////////////////

			for (int i = 0; i < deltaFrame; ++i)
			{
				 _gameLogic.Update();
			}

			if(deltaFrame > 0)
			{
				_gameView.Render(deltaFrame);
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 現在SceneView取得
		public BaseSceneView GetCurrentSceneView()
		{
			return _gameView.GetCurrentSceneView();
		}
	}
}