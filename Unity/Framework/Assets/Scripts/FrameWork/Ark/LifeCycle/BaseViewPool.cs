using System.Collections.Generic;
using UnityEngine;

namespace Ark.LifeCycle
{
	public class BaseViewPool : MonoBehaviour
	{
		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! View追加（複数）
		protected void AddView<TView>(TView viewPrefab, List<TView> list, int addNum) where TView : EntityView
		{
			for (int i = 0; i < addNum; ++i)
			{
				AddView(viewPrefab, list);
			}
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! View追加（単体）
		protected TView AddView<TView, TViewPrefab>(TViewPrefab viewPrefab, List<TView> list) where TView : class, IEntityView where TViewPrefab : EntityView
		{
			TView view = Instantiate(viewPrefab, Vector3.zero, Quaternion.identity) as TView;
			view.InitView(transform);
			view.DeactiveView();
			list.Add(view);
			return view;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! ユニットを取得する
		protected TView GetView<TView, TViewPrefab>(List<TView> list, TViewPrefab viewPrefab) where TView : class, IEntityView where TViewPrefab : EntityView
		{
			TView view = null;
			// リストからViewを取得する
			for (int i = 0; i < list.Count; ++i)
			{
				if (!list[i].IsViewActive)
				{
					view = list[i];
					break;
				}
			}
			// リストになかったらViewを作る
			if (view == null)
			{
				view = AddView(viewPrefab, list);
			}

			return view;
		}

		// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
		//! Viewを活性化する
		protected void ActiveView<TView>(TView view) where TView : class, IEntityViewOrder
		{
			view.ActiveView();
		}
	}
}