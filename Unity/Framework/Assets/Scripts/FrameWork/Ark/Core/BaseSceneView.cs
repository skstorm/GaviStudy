using Ark.Gear;
using Ark.Util;
using UnityEngine;

namespace Ark.Core
{
	public interface IBaseSceneViewOrder : IGearHolder
	{

	}

	public interface IBaseSceneView_ForUIEventRegister
	{
		void NotifyCommand(ICommand command);
	}

	public class BaseSceneView : GearHolderBehavior, IBaseSceneViewOrder, IBaseSceneView_ForUIEventRegister
	{
		private ILogicStateChnager_ForView _logicStateChanger = null;

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void StartGearProcess()
		{
			base.StartGearProcess();
			_logicStateChanger = _gear.Absorb<LogicStateChanger>(new PosInfos());

			ArkLog.Debug("BaseSceneView Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void EndGearProcess()
		{
			base.EndGearProcess();

			ArkLog.Debug("BaseSceneView End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 描画処理
		public virtual void Render(int deltaFrame)
		{

		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! コマンド通知
		public virtual void NotifyCommand(ICommand command)
		{
			ArkLog.Debug("NotifyCommand");
			_logicStateChanger.NotifyCommand(command);
		}
	}
}