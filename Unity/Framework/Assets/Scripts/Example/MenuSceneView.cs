using UnityEngine;
using Ark.Core;

namespace Example
{
	// TODO: 名称検討(Writer/reader)
	public interface IMenuSceneViewOrder : IBaseSceneViewOrder 
	{
	}

	public class MenuSceneView : BaseSceneView, IMenuSceneViewOrder
	{
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void Run()
		{
			base.Run();

			Debug.Log("MenuSceneView Run");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void DisposeProcess()
		{
			base.DisposeProcess();

			Debug.Log("MenuSceneView DisposeProcess");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 描画処理
		public override void Render(int deltaFrame)
		{
			base.Render(deltaFrame);
			//Debug.Log("MenuScene Render");
		}
	}
}
