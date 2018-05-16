using gipo.util;

namespace gipo.core
{
	/// diffuse, absorbを使うクラスの基底クラス
	/// このクラスを継承しておくことで必要な処理は基本的に自動で行われる
	/// GearHolderBehavior がほぼコピペなので、修正は両方に入れるように注意
	public class GearHolder : IGearHolder 
	{
		protected readonly bool _isRoot;
		protected Gear _gear = null;

		/// コンストラクタ
		/// Gearを必ず保持する
		/// 大親の時はisrootをtrueにすること
		public GearHolder(bool isRoot) 
		{
			_gear = new Gear(this);
			_isRoot = isRoot;

			//（Gearのセットアップ後）初期化時に行いたいこと（Action）を追加
			// Diffuseをする
			_gear.AddPreparationProcess(DiffuseGearProcess, new PosInfos());

			// prepare後に行いたいこと（Action）を追加
			// 開始処理
			// absorb可能
			_gear.AddStartProcess(StartGearProcess, new PosInfos());

			// dispose時に行いたいこと（Action）を追加
			// 修了処理
			_gear.AddEndProcess(EndGearProcess, new PosInfos());
		}

		/// Gearの外部出し用（子が親のGearを見るために必要）
		public Gear GetGear()
		{
			return _gear;
		}

		/// インスタンスが一通り揃い、Gearの親子関係ができた後の最初の処理
		/// diffuse/absorbを主導で行う場合は、addPreparationHandlerのActionに追加しておく
		public virtual void InitGear() 
		{
			_gear.Initialize();
		}

		/// GearおよびDiffuseはIDisposableなので、明示的に破棄が必要
		/// このクラスのインスタンスを破棄する場合は必ず呼ぶこと
		/// 親→子に向けて一斉にDisposeされるので注意（大親だけ呼べばいいということ）
		public void AllDisposeGear()
		{
			_gear.Dispose();
		}

		/// <summary>
		/// コンストラクタ直後に行いたいこと（Action）を追加
		/// </summary>
		protected virtual void DiffuseGearProcess()
		{
			//UnityEngine.ArkLog.Debug("ProcessBase(" + this + ")::prepare");
		}

		/// <summary>
		/// prepare後に行いたいこと（Action）を追加
		/// processSchedulerへの参照は必ずabsorbされる
		/// </summary>
		protected virtual void StartGearProcess()
		{
		}

		/// <summary>
		/// dispose時に行いたいこと（Action）を追加
		/// </summary>
		protected virtual void EndGearProcess()
		{
			//UnityEngine.ArkLog.Debug("ProcessBase(" + this + ")::disposeProcess");
		}

		/// DIコンテナのデバッグ用
		public string GearDILog() 
		{
			return _gear.DILog();
		}
	}
}
