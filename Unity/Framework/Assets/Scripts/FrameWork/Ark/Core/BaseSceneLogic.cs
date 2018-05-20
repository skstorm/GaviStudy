using Ark.Gear;
using Ark.Util;
using UnityEngine;
using System;

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
		public void CommandProcess(ICommand command)
		{
			Type commandType = command.GetType();
			if (commandType == typeof(ButtonCommand))
			{
				ButtonCommand buttonCommand = (ButtonCommand)command;
				ButtonCommandProcess(buttonCommand);
			}
			else
			{
				ArkLog.Error("処理を定義してないコマンドが来た");
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ボタンのコマンド処理
		protected void ButtonCommandProcess(ButtonCommand buttonCommand)
		{
			switch (buttonCommand.TouchKind)
			{
				case EButtonTouchKind.Tap: TapButtonCommandProcess(buttonCommand.Id);	break;
				case EButtonTouchKind.LongHold: LongHoldButtonCommandProcess(buttonCommand.Id); break;
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ボタンのコマンド処理（Tap）
		protected virtual void TapButtonCommandProcess(string commandId)
		{

		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ボタンのコマンド処理（LongHold）
		protected virtual void LongHoldButtonCommandProcess(string commandId)
		{

		}
	}
}