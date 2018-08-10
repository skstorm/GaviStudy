using UnityEngine;

namespace Ark.Core
{
	public interface IMainGameObject_ForButton
	{

	}


	abstract public class MainGameObject : MonoBehaviour, IMainGameObject_ForButton
	{
		private GameLoop _gameLoop = null;

		[SerializeField]
		protected GameView _gameView = null;

		protected DataLoadManager _dataLoadManager = null;

		// Use this for initialization
		abstract protected void Start();

		protected void Initialize (ISetting setting) 
		{
			ArkLog.Init(setting);

			_dataLoadManager = gameObject.AddComponent<DataLoadManager>();
			_dataLoadManager.Init(setting.BundleUrl);
			_dataLoadManager.LoadAllAsset();

			_gameLoop = new GameLoop(setting, _gameView, _dataLoadManager);
			_gameLoop.InitGear();
		}

		// Update is called once per frame
		void Update()
		{
			_gameLoop.Update();
		}

		void OnApplicationQuit()
		{
			_dataLoadManager.Release();
			_gameLoop.AllDisposeGear();
			ArkLog.Release();
		}
		
		public BaseSceneView GetCurrentSceneView()
		{
			return _gameLoop.GetCurrentSceneView();
		}
	}
}
