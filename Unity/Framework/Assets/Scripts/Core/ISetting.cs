using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Example;

public interface ISetting
{
	// 開始シーン
	IBaseSceneLogic StartScene { get; }
	// Frame per sencod (1秒に回るFrame数)
	int Fps { get; }
}
