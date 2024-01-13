using Ark.Core;
using UnityEngine;

namespace Example
{
	public class MenuSceneLogic : BaseSceneLogic<MenuSceneLogic ,IMenuSceneViewOrder>
	{

		public override void Update()
		{
			base.Update();
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化
		protected override void StartNodeProcess()
		{
			base.StartNodeProcess();

			ArkLog.Debug("MenuSceneLogic Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndNodeProcess()
		{
			base.EndNodeProcess();

			ArkLog.Debug("MenuSceneLogic End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ボタンのコマンド処理（Tap）
		protected override void TapButtonCommandProcess(string commandId)
		{
			base.TapButtonCommandProcess(commandId);

			if (commandId == MenuSceneCommandDefine.GoToBattle)
			{
				_gameLogic.ChangeScene(new BattleSceneLogic());
			}
		}
	}
}