using gipo.util;

namespace gipo.core.handler
{
	/// Gear用Handlerを保持するクラス
	/// 追加された場所も保持
	public class GearDispatcherHandler<TFunc> : CancelKey
	{
		public TFunc _func;
		public PosInfos _addPos;
		
		public GearDispatcherHandler(TFunc func, PosInfos addPos)
		{
			_func = func;
			_addPos = addPos;
		}
		
		public override string ToString()
		{
			return string.Format("[Handler {0} {1}]", _addPos, typeof(TFunc));
		}
	}

}