using Core;

namespace Example
{
	public enum ESceneKind
	{
		Menu = 0,
		Battle,
		Gacha
	}

	public class ExampleTool
	{
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! シーン生成
		public static IBaseSceneLogic CreateScene(ESceneKind sceneKind)
		{
			IBaseSceneLogic sceneLogic = null;

			switch (sceneKind)
			{
				case ESceneKind.Menu: break;
				case ESceneKind.Battle: break;
				case ESceneKind.Gacha: break;
			}

			return sceneLogic;
		}
	}
}