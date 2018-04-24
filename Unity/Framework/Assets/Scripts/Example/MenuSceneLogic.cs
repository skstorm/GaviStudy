using Core;
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
		protected override void Run()
		{
			base.Run();

			Debug.Log("MenuSceneLogic Run");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void DisposeProcess()
		{
			base.DisposeProcess();

			Debug.Log("MenuSceneLogic DisposeProcess");
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