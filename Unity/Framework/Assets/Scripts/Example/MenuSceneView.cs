using Ark.Core;

namespace Example
{
	// TODO: 名称検討(Writer/reader)
	public interface IMenuSceneViewOrder : IBaseSceneViewOrder 
	{
	}

	public class MenuSceneView : BaseSceneView<MenuSceneView>, IMenuSceneViewOrder
	{
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void StartNodeProcess()
		{
			base.StartNodeProcess();

			ArkLog.Debug("MenuSceneView Start");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void EndNodeProcess()
		{
			base.EndNodeProcess();

			ArkLog.Debug("MenuSceneView End");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 描画処理
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
			//ArkLog.Debug("MenuScene Render");
		}
	}
}
