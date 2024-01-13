using Ark.Core;
using Ark.DiTree;
using DiTreeGroup;

namespace Example
{
	public class MyMainGameObject : MainGameObject
	{
		// Use this for initialization
		protected override void Start()
		{
            DiTreeInitializer.InitDiTree<ArkDiTree<IDiField>>();
            Initialize(new MySetting("MenuSceneLogic"));
		}
	}
}