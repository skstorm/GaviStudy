using gipo.util;
using System;
using System.Collections.Generic;

namespace gipo.core.handler
{
	/// TFuncは引数なし→戻り値なし。よってAction。
	/// Gear用Handlerを実行するDispatcher本体
	public class GenericGearDispatcher<TFunc> {
		private List<GearDispatcherHandler<TFunc>> list;

		private AddBehavior.MethodType addBehavior;
		private bool executeLock;
		private bool once;
		private PosInfos dispatcherPos;
		public  PosInfos DispatcherPos { get { return dispatcherPos; } }

		/// 初期化
		public GenericGearDispatcher(AddBehavior.MethodType addBehavior, bool once, PosInfos dispatcherPos) {
			this.addBehavior = addBehavior;
			this.once = once;
			this.dispatcherPos = dispatcherPos;
			
			executeLock = false;
			clear();
		}

		/// 実行すべきHandlerのクリア
		private void clear() {
			if (list == null) {
				list = new List<GearDispatcherHandler<TFunc>>();
			} else {
				list.Clear();
			}
		}

		/// Handler追加（削除用のCancelKeyが返る）
		public CancelKey add(TFunc func, PosInfos addPos) {
			GearDispatcherHandler<TFunc> handler = new GearDispatcherHandler<TFunc>(func, addPos);
			AddBehavior.execute(addBehavior, list, handler);
			return handler;
		}

		/// Handler削除（追加時に得たCancelKeyを使う）
		public void remove(CancelKey key) {
			if (executeLock) {
				throw new Exception("実行最中の削除はできません");
			}
			GearDispatcherHandler<TFunc> rmhandler = null;
			foreach(GearDispatcherHandler<TFunc> handler in list) {
				if ((CancelKey)handler == key) {
					rmhandler = handler;
					break;
				}
			}
			if (rmhandler != null) {
				list.Remove(rmhandler);
			}
		}

		/// 実行
		/// 同時実行されたときにおかしくならないようにいったんローカルに対比してから実行している
		public void execute(Action<GearDispatcherHandler<TFunc>> treat, PosInfos executePos) {
			if (executeLock) {
				throw new Exception("実行関数が入れ子になっています");
			}
			executeLock = true;
			List<GearDispatcherHandler<TFunc>> tmpHandlerList = new List<GearDispatcherHandler<TFunc>>(list);

			if (once) {
				clear();
			}

			for (int i=0; i<tmpHandlerList.Count; i++) {
				treat(tmpHandlerList[i]);
			}

			tmpHandlerList.Clear();
			tmpHandlerList = null;
			executeLock = false;
		}

	}
}
