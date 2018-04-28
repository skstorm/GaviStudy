﻿using UnityEngine;

namespace Core
{
	public interface IMainGameObject_ForButton
	{

	}


	abstract public class MainGameObject : MonoBehaviour, IMainGameObject_ForButton
	{
		private GameLoop _gameLoop = null;

		[SerializeField]
		protected GameView _gameView;

		// Use this for initialization
		abstract protected void Start();

		protected void Initialize (ISetting setting) 
		{
			_gameLoop = new GameLoop(setting, _gameView);
			_gameLoop.GearInit();
		}

		// Update is called once per frame
		void Update()
		{
			_gameLoop.Update();
		}

		void OnApplicationQuit()
		{
			_gameLoop.GearDispose();	
		}
		
		public BaseSceneView GetCurrentSceneView()
		{
			return _gameLoop.GetCurrentSceneView();
		}
	}
}
