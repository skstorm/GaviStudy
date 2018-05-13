
namespace Ark.Core
{
	public interface ISetting
	{
		// 開始シーン
		IBaseSceneLogic StartScene { get; }
		// Frame per sencod (1秒に回るFrame数)
		int Fps { get; }
		// 表示するログレベル
		uint DisplayLogLevel { get; }
	}
}