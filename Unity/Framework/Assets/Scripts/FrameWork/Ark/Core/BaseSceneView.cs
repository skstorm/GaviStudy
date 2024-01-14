using Ark.DiTree;
using Ark.Gear;
using Ark.Util;
using DiTreeGroup;
using UnityEngine;

namespace Ark.Core
{
	public interface IBaseSceneViewOrder : IDiTreeHolderBehavior
	{

	}

	public interface IBaseSceneView_ForUIEventRegister
	{
		void NotifyCommand(ICommand command);
	}

	public interface IBaseSceneView : IBaseSceneViewOrder, IBaseSceneView_ForUIEventRegister
    {
		void Render(int deltaFrame);

		GameObject GameObject { get; }

    }


    public class BaseSceneView<TDiTreeHolder> : ArkDiTreeHolderBehavior<TDiTreeHolder>, IBaseSceneView
        where TDiTreeHolder : class, IDiField
    {
		private ILogicStateChnager_ForView _logicStateChanger = null;

		public GameObject GameObject => gameObject;

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void StartNodeProcess()
		{
			base.StartNodeProcess();
			_logicStateChanger = _tree.Get<LogicStateChanger>();

			ArkLog.Debug("BaseSceneView Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void EndNodeProcess()
		{
			base.EndNodeProcess();

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