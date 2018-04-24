using System;
using System.Collections.Generic;

using gipo.util;

namespace gipo.core.handler
{
	/// Gear用Handlerを保持するクラス
	/// 追加された場所も保持
	public class GearDispatcherHandler<TFunc> : CancelKey {
		public TFunc func;
		public PosInfos addPos;
		
		public GearDispatcherHandler(TFunc func, PosInfos addPos) {
			this.func = func;
			this.addPos = addPos;
		}
		
		public override string ToString() {
			return string.Format("[Handler {0} {1}]", addPos, typeof(TFunc));
		}
	}

}