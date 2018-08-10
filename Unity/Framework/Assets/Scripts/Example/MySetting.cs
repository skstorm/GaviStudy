using Ark.Core;

namespace Example
{
	public class MySetting : ISetting
	{
		private readonly IBaseSceneLogic _startScene;
		public IBaseSceneLogic StartScene
		{
			get
			{
				return _startScene;
			}
		}

		public int Fps
		{
			get
			{
				return 60;
			}
		}

		private readonly uint _displayLogLevel = 0;
		public uint DisplayLogLevel
		{
			get
			{
				return _displayLogLevel;
			}
		}

		private readonly string _bundleUrl = "file:///D:/Work/Project/GaviStudy/Unity/Framework/Assets/AssetBundles/test_asset_bundle";
		public string BundleUrl
		{
			get
			{
				return _bundleUrl;
			}
		}

		public MySetting()
		{
			_startScene = new MenuSceneLogic();
			_displayLogLevel = ArkLogLevelDefine.Error | ArkLogLevelDefine.Warning | ArkLogLevelDefine.Info | ArkLogLevelDefine.Debug;
		}
	}
}