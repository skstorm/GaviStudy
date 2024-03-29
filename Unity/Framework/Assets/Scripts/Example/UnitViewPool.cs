﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ark.Core;
using Ark.LifeCycle;

namespace Example
{
	public class UnitViewPool : BaseViewPool
	{
		private static UnitViewPool s_instance;

		private const string PATH_TEST_UNIT_VIEW = "TestUnit";

		private EntityView _testUnitViewPrefab;
		
		private List<EntityView> _blackSplashViews = new List<EntityView>();

		static public UnitViewPool GetInstance()
		{
			return s_instance;
		}

		void Awake()
		{
			s_instance = this;
		}

		void OnDestroy()
		{
		}

		public void Init(DataLoadManager dataloadManager)
		{
			_testUnitViewPrefab = dataloadManager.GetAsset(PATH_TEST_UNIT_VIEW).GetComponent<EntityView>();
			const int TEST_UNIT_NUM = 50;
			AddView(_testUnitViewPrefab, _blackSplashViews, TEST_UNIT_NUM);
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Viewを取得する （Viewの初期化も行う）
		public IEntityView GetView(TestUnitLogic.EKind kind)
		{
			IEntityView view = null;
			// View取得
			switch (kind)
			{
				case TestUnitLogic.EKind.NormalUnit: view = GetView(_blackSplashViews, _testUnitViewPrefab); break;
				default:
					throw new ArgumentOutOfRangeException("EKind", kind, null);
			}
			// Viewの初期化
			ActiveView(view);

			return view;
		}
	}
}