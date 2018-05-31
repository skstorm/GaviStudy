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
			
			ArkLog.Debug("BattleSceneLogic Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			ArkLog.Debug("BattleSceneLogic End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ボタンのコマンド処理（Tap）
		protected override void TapButtonCommandProcess(string commandId)
		{
			base.TapButtonCommandProcess(commandId);

			if (commandId == BattleSceneCommandDefine.GoToMenu)
			{
				_gameLogic.ChangeScene(new MenuSceneLogic());
			}
		}
	}
}