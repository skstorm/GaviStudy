using gipo.util;
using System;
using System.Collections.Generic;

namespace gipo.core.handler
{
	/// TFuncは引数なし→戻り値なし。よってAction。
	/// Gear用Handlerを実行するDispatcher本体
	public class GenericGearDispatcher<TFunc>
	{
		private List<GearDispatcherHandler<TFunc>> _list;

		private readonly AddBehavior.MethodType _addBehavior;
		private bool _executeLock;
		private readonly bool _once;
		private readonly PosInfos _dispatcherPos;
		public  PosInfos DispatcherPos { get { return _dispatcherPos; } }

		/// 初期化
		public GenericGearDispatcher(AddBehavior.MethodType addBehavior, bool once, PosInfos dispatcherPos)
		{
			_addBehavior = addBehavior;
			_once = once;
			_dispatcherPos = dispatcherPos;
			
			_executeLock = false;
			_list = new List<GearDispatcherHandler<TFunc>>();
		}

		/// 実行すべきHandlerのクリア
		private void Clear()
		{
			_list.Clear();
		}

		/// Handler追加（削除用のCancelKeyが返る）
		public CancelKey Add(TFunc func, PosInfos addPos)
		{
			GearDispatcherHandler<TFunc> handler = new GearDispatcherHandler<TFunc>(func, addPos);
			AddBehavior.Execute(_addBehavior, _list, handler);
			return handler;
		}

		/// Handler削除（追加時に得たCancelKeyを使う）
		public void Remove(CancelKey key)
		{
			if (_executeLock)
			{
				throw new Exception("実行最中の削除はできません");
			}

			GearDispatcherHandler<TFunc> rmhandler = null;
			foreach(GearDispatcherHandler<TFunc> handler in _list)
			{
				if ((CancelKey)handler == key)
				{
					rmhandler = handler;
					break;
				}
			}

			if (rmhandler != null)
			{
				_list.Remove(rmhandler);
			}
		}

		/// 実行
		/// 同時実行されたときにおかしくならないようにいったんローカルに対比してから実行している
		public void Execute(Action<GearDispatcherHandler<TFunc>> treat, PosInfos executePos)
		{
			if (_executeLock)
			{
				throw new Exception("実行関数が入れ子になっています");
			}

			_executeLock = true;
			List<GearDispatcherHandler<TFunc>> tmpHandlerList = new List<GearDispatcherHandler<TFunc>>(_list);

			if (_once)
			{
				Clear();
			}

			for (int i=0; i<tmpHandlerList.Count; i++)
			{
				treat(tmpHandlerList[i]);
			}

			tmpHandlerList.Clear();
			tmpHandlerList = null;
			_executeLock = false;
		}

	}
}
