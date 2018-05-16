using Ark.Core;
using UnityEngine;

namespace Example
{
	public interface IBattleSceneViewOrder : IBaseSceneViewOrder
	{
	}
	public class BattleSceneView : BaseSceneView, IBattleSceneViewOrder
	{
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			ArkLog.Debug("BattleSceneView Run");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			ArkLog.Debug("BattleSceneView DisposeProcess");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 描画処理
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
		}
	}
}