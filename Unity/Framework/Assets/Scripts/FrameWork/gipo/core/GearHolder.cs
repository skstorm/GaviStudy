using System;
using System.Reflection;

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

		/// Gearがprepare->runまで済んでいるか？
		public bool Initialized 
		{
			get 
			{
				if (_gear == null) return false;
				return _gear.CheckPhaseCanMiddleTool();
			}
		}

		/// コンストラクタ
		/// Gearを必ず保持する
		/// 大親の時はisrootをtrueにすること
		public GearHolder(bool isRoot) 
		{
			_gear = new Gear(this);
			_isRoot = isRoot;

			FieldSetup();

			//（Gearのセットアップ後）初期化時に行いたいこと（Action）を追加
			// Attribute属性を用いていない場合はここでdiffuse/absorbをAction化しておくといい感じ？
			_gear.AddPreparationHandler(GearDiffuse, new PosInfos());

			// prepare後に行いたいこと（Action）を追加
			_gear.AddRunHandler(Run, new PosInfos());

			// dispose時に行いたいこと（Action）を追加
			_gear.AddDisposeProcess(DisposeProcess, new PosInfos());
		}

		/// attributeにてdiffuseされるメンバ変数はautoGearSetup前にインスタンスが生成されている必要があるので
		/// このメソッドでコンストラクタでのautoGearSetup前に割り込める
		protected virtual void FieldSetup() 
		{
			// 継承先でのコーディング例
			// hoge = new Hoge();
		}

		/// インスタンスが一通り揃い、Gearの親子関係ができた後の最初の処理
		/// diffuse/absorbを主導で行う場合は、addPreparationHandlerのActionに追加しておく
		public virtual void GearInit() 
		{
			_gear.Initialize();
		}

		/// <summary>
		/// コンストラクタ直後に行いたいこと（Action）を追加
		/// </summary>
		protected virtual void GearDiffuse()
		{
			//UnityEngine.ArkLog.Debug("ProcessBase(" + this + ")::prepare");
		}

		/// <summary>
		/// prepare後に行いたいこと（Action）を追加
		/// processSchedulerへの参照は必ずabsorbされる
		/// </summary>
		protected virtual void Run()
		{
		}

		/// <summary>
		/// dispose時に行いたいこと（Action）を追加
		/// </summary>
		protected virtual void DisposeProcess()
		{
			//UnityEngine.ArkLog.Debug("ProcessBase(" + this + ")::disposeProcess");
		}

		/// Gearの外部出し用（子が親のGearを見るために必要）
		public Gear GetGear() 
		{
			return _gear;
		}

		/// GearおよびDiffuseはIDisposableなので、明示的に破棄が必要
		/// このクラスのインスタンスを破棄する場合は必ず呼ぶこと
		/// 親→子に向けて一斉にDisposeされるので注意（大親だけ呼べばいいということ）
		public void GearDispose() 
		{
			_gear.Dispose();
		}

		/// DIコンテナのデバッグ用
		public string GearDILog() 
		{
			return _gear.DILog();
		}
	}
}
