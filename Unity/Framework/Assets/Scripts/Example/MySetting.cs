using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Example;

public class MySetting : ISetting {

	public IBaseSceneLogic StartScene
	{
		get
		{
			return new MenuSceneLogic ();
		}
	}
}
