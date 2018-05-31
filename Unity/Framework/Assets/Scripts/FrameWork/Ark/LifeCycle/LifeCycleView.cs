using System.Collections.Generic;
using UnityEngine;

namespace Ark.LifeCycle
{
	public class LifeCycleView
	{
		protected Dictionary<IEntityLogicPeek, IEntityView> _entities = new Dictionary<IEntityLogicPeek, IEntityView>();

		protected IEntityFieldView_ForEntityView _entityFieldView = null;

		public LifeCycleView(IEntityFieldView_ForEntityView entityFieldView)
		{
			_entityFieldView = entityFieldView;
		}

		public void Update(int deltaFrame)
		{
			// 全バトルオブジェクトの描画更新
			var enumerator = _entities.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Value.UpdateView(deltaFrame);
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		public void AddEntity(IEntityLogicPeek logic, IEntityView view)
		{
			view.Init(_entityFieldView, logic);
			_entities.Add(logic, view);
		}

		public void RemoveEntity(IEntityLogicPeek obj)
		{
			if (_entities.ContainsKey(obj))
			{
				IEntityView view = _entities[obj];
				if (view.IsPool)
				{
					view.DeactiveView();
				}
				else
				{
					Object.Destroy(view.GameObject);
				}

				_entities.Remove(obj);
			}
		}
	}
}