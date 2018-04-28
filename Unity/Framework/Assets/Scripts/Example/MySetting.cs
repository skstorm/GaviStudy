using Ark.Core;

namespace Example
{
	public class MySetting : ISetting
	{
		public IBaseSceneLogic StartScene
		{
			get
			{
				return new MenuSceneLogic();
			}
		}

		public int Fps
		{
			get
			{
				return 60;
			}
		}
	}

}