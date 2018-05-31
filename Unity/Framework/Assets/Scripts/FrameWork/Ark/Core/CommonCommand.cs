namespace Ark.Core
{
	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! すべてのコマンドの共通Interface
	public interface ICommand
	{
	}

	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! ボタンのタッチ種類
	public enum EButtonTouchKind
	{
		Tap,
		LongHold
	}

	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! ボタン用のコマンド
	public class ButtonCommand : ICommand
	{
		// Id（ボタン識別用）
		private readonly string _id;
		public string Id { get { return _id; } }
		// ボタンのタッチ種類
		private readonly EButtonTouchKind _touchKind;
		public EButtonTouchKind TouchKind { get { return _touchKind; } }

		public ButtonCommand(string id, EButtonTouchKind touchKind)
		{
			_id = id;
			_touchKind = touchKind;
		}
	}


}