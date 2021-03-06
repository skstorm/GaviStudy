﻿using UnityEngine;
using Ark.Core;

namespace Example
{
	public class MyGameView : GameView
	{
		[SerializeField]
		private MenuSceneView _menuSceneView = null;
		[SerializeField]
		private BattleSceneView _battleSceneView = null;

		protected override BaseSceneView CreateSceneView(IBaseSceneLogic sceneLogic)
		{
			if (sceneLogic.GetType() == typeof(MenuSceneLogic))
			{
				//return Instantiate<MenuSceneView>()
				return Instantiate(_menuSceneView, transform);
			}
			else if(sceneLogic.GetType() == typeof(BattleSceneLogic))
			{
				return Instantiate(_battleSceneView, transform);
			}

			return null;
		}
	}
}