﻿using gipo.core;
using gipo.util;
using UnityEngine;

namespace Core
{
	public interface IBaseSceneViewOrder : IGearHolder
	{

	}

	public interface IBaseSceneView_ForUIEventRegister
	{
		void NotifyCommand(ICommand command);
	}

	public class BaseSceneView : GearHolderBehavior, IBaseSceneViewOrder, IBaseSceneView_ForUIEventRegister
	{
		private ILogicStateChnager_ForView _logicStateChanger = null;

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 初期化関数
		protected override void Run()
		{
			base.Run();
			_logicStateChanger = _gear.Absorb<LogicStateChanger>(new PosInfos());

			Debug.Log("BaseSceneView Run");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 解除処理
		protected override void DisposeProcess()
		{
			base.DisposeProcess();

			Debug.Log("BaseSceneView DisposeProcess");
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! 描画処理
		public virtual void Render(int deltaFrame)
		{

		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! コマンド通知
		public virtual void NotifyCommand(ICommand command)
		{
			Debug.Log("NotifyCommand");
			_logicStateChanger.NotifyCommand(command);
		}
	}
}