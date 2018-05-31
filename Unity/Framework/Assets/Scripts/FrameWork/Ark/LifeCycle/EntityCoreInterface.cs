
namespace Ark.LifeCycle
{
	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! 移動可能なObject
	public interface IMovable
	{
		// 移動
		void Move();
		// ユニットを壁の中に閉じ込める
		void ConfineInWall(float leftWall, float topWall, float rightWall, float bottomWall);
		// 生きているかどうか（一部の関数は死んでたら動作しないため…　上位Interfaceにあげる必要がある？要検討）
		bool IsLive();
		// 自分がどの武将に所属されているかを判定するための変数
		bool IsOutsideMap { get; }
		// 壁とのチェックを行うか
		bool IsCheckWall { get; }
	}

	// -=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=- //
	//! 更新可能Object
	//　更新するもの：自分の状態
	public interface IUpdatable
	{
		// 更新（主に状態変更周り　毎フレ回る）
		void Update();
		// 更新（一定Frame間隔で行われる）
		void PeriodicUpdate();
		// 消すべきか？
		bool IsWillRemove { get; }
		bool IsWillChangeView { get; }
		void ChangeView(IEntityViewOrder view);
		IEntityViewOrder GetWillChangeView();
		void SetView(IEntityViewOrder view);
		void EndChangeView();
		void UpdateChangeState();
	}
}