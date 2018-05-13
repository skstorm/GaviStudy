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
		//protected CyLog log = new CyLog();

		/// diffuse/absorbの自動化フラグ（c#はdefineが不便なのでこちらで...）
		public static readonly bool _autoDiffuse = false;
		public static readonly bool _autoAbsorb = false;

		protected bool _isRoot;
		protected Gear _gear;

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
			AutoGearSetup();

			if (_autoAbsorb) 
			{
				AutoAbsorb(isRoot);
			}

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

		/// gearの親子関係を自動で設定する
		/// thisのフィールド（メンバ変数）を探索し、GearHolderを継承している場合に親子関係を築く
		/// またカスタムアトリビュート [Diffuse] がついているフィールドがある場合はdiffuseも行う
		private void AutoGearSetup() 
		{
			// 呼び出し元のField（メンバ変数）情報をprivateも含めて取得する
			Type t = this.GetType();
			foreach(FieldInfo fi in t.GetFields(BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance)) 
			{
				// GearHolder派生クラスならセットアップ処理
				if (fi.FieldType.IsSubclassOf(typeof(GearHolder))) 
				{
					// インスタンス取得（IGearHolderインターフェイスを持ってるはず）
					IGearHolder val = (IGearHolder)fi.GetValue(this);
					// Gearセットアップ処理（子がインスタンス生成されていたら）
					if (val != null) 
					{
						_gear.AddChildGear(val.GetGear());
						if (_autoDiffuse) 
						{
							_gear.Diffuse(val, fi.FieldType);
						}
					}
				} 
				else 
				{
					if (_autoDiffuse) 
					{
						// Field情報の属性（Attribute）を取得
						object[] attributes = fi.GetCustomAttributes(true);
						foreach(Attribute at in attributes) 
						{
							// Diffuse属性がついているFieldに対して実行
							if (at is DiffuseAttribute) 
							{
								// インスタンス取得してdiffuse
								object val = fi.GetValue(this);
								if (val != null) 
								{
									_gear.Diffuse(val, fi.FieldType);
								}
							}
						}
					}
				}
			}
		}

		/// 自クラスでカスタムアトリビュート [Absorb] がついているフィールドを探して設定する
		/// Reflectionを使いまくっているのでJITでの動作は怪しいかもしれない（その場合は手動に切り替え）
		public void AutoAbsorb(bool isRoot) 
		{
			// 大親から以外のautoAbsorbは無視
			if (!isRoot) return;
			// 呼び出し元のField（メンバ変数）情報をprivateも含めて取得する
			Type t = this.GetType();
			foreach(FieldInfo fi in t.GetFields(BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance)) 
			{
				// Field情報の属性（Attribute）を取得
				object[] attributes = fi.GetCustomAttributes(true);
				foreach(Attribute at in attributes) 
				{
					// Diffuse属性がついているFieldに対して実行
					if (at is AbsorbAttribute) 
					{
						// インスタンス取得（IGearHolderインターフェイスを持ってるはず）
						IGearHolder val = (IGearHolder)fi.GetValue(this);
						// 未設定ならabsorb
						if (val == null) 
						{
							MethodInfo gmi = typeof(Gear).GetMethod("absorb", BindingFlags.Public | BindingFlags.Instance).MakeGenericMethod(fi.FieldType);
							fi.SetValue(this, gmi.Invoke(_gear, new object[]{ new PosInfos() }));
							UnityEngine.Debug.Log("autoAbsorb : " + fi.FieldType + " of " + this);
						}
					} 
					else if (at is DiffuseAttribute) 
					{
						// インスタンス取得（IGearHolderインターフェイスを持ってるはず）
						IGearHolder val = (IGearHolder)fi.GetValue(this);
						val.AutoAbsorb(isRoot); // 大親～子孫へ向かって順にautoAbsorbする
					}
				}
			}
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
