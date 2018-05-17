using Ark.Core;
using UnityEngine;

namespace Example
{
	public class MenuSceneLogic : BaseSceneLogic<IMenuSceneViewOrder>
	{

		public override void Update()
		{
			base.Update();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			ArkLog.Debug("MenuSceneLogic Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			ArkLog.Debug("MenuSceneLogic End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! コマンド処理
		public override void CommandProcess(ICommand command)
		{
			base.CommandProcess(command);

			if(command.GetType() == typeof(ButtonCommand))
			{
				ButtonCommand buttonCommand = (ButtonCommand)command;
				switch(buttonCommand.TouchKind)
				{
					case EButtonTouchKind.Tap:
						if (buttonCommand.Id == MenuSceneCommandDefine.GoToBattle)
						{
							_gameLogic.ChangeScene(new BattleSceneLogic());
						}
						break;
				}
			}
		}
	}
}