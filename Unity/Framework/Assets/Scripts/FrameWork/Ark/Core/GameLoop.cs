﻿using UnityEngine;
using Ark.Gear;
using Ark.Util;

namespace Ark.Core
{
	public class GameLoop : GearHolder
	{
		private FrameManager _frameManager = null;
		private GameLogic _gameLogic = null;
		private GameView _gameView = null;
		private LogicStateChanger _logicStateChanger = null;
		private CommandRecorder _commandRecorder = null;
		private CommandReplayer _commandReplayer = null;
		private DataLoadManager _dataLoadManager = null;

		private float _debugLastUpdateSeconds = 0;
		private float _debugDeltaFrame = 0;
		private int _debugCount = 0;

		public GameLoop(ISetting setting, GameView gameView, DataLoadManager dataLoadManager) : base(true)
		{
			_frameManager = new FrameManager(setting.Fps);
			_gameLogic = new GameLogic(setting);
			_gameView = gameView;
			_gameView.InitDI(false);
			_logicStateChanger = new LogicStateChanger();
			_commandRecorder = new CommandRecorder();
			_commandReplayer = new CommandReplayer();
			_dataLoadManager = dataLoadManager;

			_gear.AddChildGear(_frameManager.GetGear());
			_gear.AddChildGear(_gameLogic.GetGear());
			_gear.AddChildGear(_gameView.GetGear());
			_gear.AddChildGear(_logicStateChanger.GetGear());
			_gear.AddChildGear(_commandRecorder.GetGear());
			_gear.AddChildGear(_commandReplayer.GetGear());
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 子供にギアを拡散する
		protected override void DiffuseGearProcess()
		{
			base.DiffuseGearProcess();
			
			_gear.Diffuse(_frameManager, typeof(FrameManager));
			_gear.Diffuse(_gameLogic, typeof(GameLogic));
			_gear.Diffuse(_gameView, typeof(GameView));
			_gear.Diffuse(_logicStateChanger, typeof(LogicStateChanger));
			_gear.Diffuse(_commandRecorder, typeof(CommandRecorder));
			_gear.Diffuse(_commandReplayer, typeof(CommandReplayer));
			_gear.Diffuse(_dataLoadManager, typeof(DataLoadManager));
		}


		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			ArkLog.Debug("Game Loop Start");

			// FrameManagerの時間初期化
			_frameManager.RecordLastUpdateSeconds();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndGearProcess()
		{
			base.EndGearProcess();
			ArkLog.Debug("Game Loop End");
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
				//ArkLog.Debug("count : "+ _debugCount);
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