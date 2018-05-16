using Ark.Core;
using UnityEngine;

namespace Example
{
	public class BattleSceneLogic : BaseSceneLogic<IBattleSceneViewOrder>
	{
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 更新処理
		public override void Update()
		{
			base.Update();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			
			ArkLog.Debug("BattleSceneLogic Run");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			ArkLog.Debug("BattleSceneLogic DisposeProcess");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! コマンド処理
		public override void CommandProcess(ICommand command)
		{
			base.CommandProcess(command);

			if (command.GetType() == typeof(ButtonCommand))
			{
				ButtonCommand buttonCommand = (ButtonCommand)command;
				switch (buttonCommand.TouchKind)
				{
					case EButtonTouchKind.Tap:
						if (buttonCommand.Id == BattleSceneCommandDefine.GoToMenu)
						{
							_gameLogic.ChangeScene(new MenuSceneLogic());
						}
						break;
				}
			}
		}
	}
}