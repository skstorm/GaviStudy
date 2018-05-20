using System;
using gipo.util;

namespace gipo.core.handler
{
	/// Gear用Handlerを実行するDispatcher
	/// 追加・削除・実行
	class GearDispatcher
	{
		/// 実行してくれる本体
		private GenericGearDispatcher<Action> GenericGearDispatcher;

		/// 初期化
		public GearDispatcher(AddBehavior.MethodType addBehavior, bool once, PosInfos pos)
		{
			GenericGearDispatcher = new GenericGearDispatcher<Action>(addBehavior, once, pos);
		}

		/// 追加（削除用のCancelKeyが返る）
		public CancelKey Add(Action func, PosInfos addPos)
		{
			return GenericGearDispatcher.Add(func, addPos);
		}

		/// 削除
		public void Remove(CancelKey key)
		{
			GenericGearDispatcher.Remove(key);
		}

		/// 実行
		public void Execute(PosInfos executePos)
		{
			GenericGearDispatcher.Execute(Trat, executePos);
		}

		/// 実行本体
		private void Trat(GearDispatcherHandler<Action> handler)
		{
			handler._func();
		}
	}
}
