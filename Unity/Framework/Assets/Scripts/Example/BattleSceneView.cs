using Ark.Core;
using Ark.Util;
using UnityEngine;

namespace Example
{
	public interface IBattleSceneViewOrder : IBaseSceneViewOrder
	{
	}
	public class BattleSceneView : BaseSceneView, IBattleSceneViewOrder
	{
		private DataLoadManager _dataLoadManager = null;

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void StartGearProcess()
		{
			base.StartGearProcess();

			ArkLog.Debug("BattleSceneView Start");

			_dataLoadManager = _gear.Absorb<DataLoadManager>(new PosInfos());

			UnitViewPool.GetInstance().Init(_dataLoadManager);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			ArkLog.Debug("BattleSceneView End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 描画処理
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
		}
	}
}