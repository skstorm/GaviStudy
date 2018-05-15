using System;
using System.Reflection;
using UnityEngine;
using gipo.util;

namespace gipo.core
{
	/// <summary>
	/// diffuse, absorbを使うクラスの基底クラスのMonoBehavior対応版
	/// コンストラクタは使えないし、StartとAwakeは実行タイミングが怪しいので
	/// 初期化は共通化していない。
	/// 
	/// なので以下の感じで手動で叩いて下さい。
	///   GearHolderBehaviorTest ghb = uiPanelManager.GetComponent<GearHolderBehaviorTest>("GHB");
	///	  ghb.InitDI(gear);
	///	Start() 内でInitDI するなら、参照時に Initialized を確認したほうが良いかもです。
	///	
	///	ちなみに ghb.InitDI(); とすれば、大親としてGearのツリーを作れます。
	/// </summary>
	public class GearHolderBehavior : MonoBehaviour, IGearHolder
	{
		protected bool _isRoot = false;
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

		/// <summary>
		/// コンストラクタは使えないので、使うときは手動でこの関数をた叩いて下さい。
		/// </summary>
		public void InitDI(bool isRoot)
		{
			_gear = new Gear(this);

			_isRoot = isRoot;

			FieldSetup();

			//（Gearのセットアップ後）初期化時に行いたいこと（Action）を追加
			// Attribute属性を用いていない場合はここでdiffuse/absorbをAction化しておくといい感じ？
			_gear.AddPreparationHandler(GearDiffuse, new PosInfos());

			// prepare後に行いたいこと（Action）を追加
			_gear.AddRunHandler(Run, new PosInfos());

			// run後に行いたいこと（Action）を追加
			// 利用されない前提なのでコメントアウト
			// gear.addBubbleHandler(bubble, new PosInfos());

			// dispose時に行いたいこと（Action）を追加
			_gear.AddDisposeProcess(DisposeProcess, new PosInfos());
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
