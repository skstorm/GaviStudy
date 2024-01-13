using Ark.Core;
using Ark.Util;
using UnityEngine;

namespace Example
{
	public interface IBattleSceneViewOrder : IBaseSceneViewOrder
	{
	}
	public class BattleSceneView : BaseSceneView<BattleSceneView>, IBattleSceneViewOrder
	{
		private DataLoadManager _dataLoadManager = null;

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void StartNodeProcess()
		{
			base.StartNodeProcess();

			ArkLog.Debug("BattleSceneView Start");

			_dataLoadManager = _tree.Get<DataLoadManager>();

			//UnitViewPool.GetInstance().Init(_dataLoadManager);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void EndNodeProcess()
		{
			base.EndNodeProcess();

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