using System;
using System.Collections.Generic;

using gipo.util;

namespace gipo.core.handler
{
	/// Gear用Handlerを実行するDispatcher
	/// 追加・削除・実行
	class GearDispatcher {
		/// 実行してくれる本体
		private GenericGearDispatcher<Action> genericGearDispatcher;

		/// 初期化
		public GearDispatcher(AddBehavior.MethodType addBehavior, bool once, PosInfos pos) {
			genericGearDispatcher = new GenericGearDispatcher<Action>(addBehavior, once, pos);
		}

		/// 追加（削除用のCancelKeyが返る）
		public CancelKey add(Action func, PosInfos addPos) {
			return genericGearDispatcher.add(func, addPos);
		}

		/// 削除
		public void remove(CancelKey key) {
			genericGearDispatcher.remove(key);
		}

		/// 実行
		public void execute(PosInfos executePos) {
			genericGearDispatcher.execute(trat, executePos);
		}

		/// 実行本体
		private void trat(GearDispatcherHandler<Action> handler)
		{
			handler.func();
		}
	}
}
