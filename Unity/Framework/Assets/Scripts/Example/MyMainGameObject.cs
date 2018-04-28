using Ark.Core;

namespace Example
{
	public class MyMainGameObject : MainGameObject
	{
		// Use this for initialization
		protected override void Start()
		{
			Initialize(new MySetting());
		}
	}
}