using System;
using System.Collections.Generic;

using gipo.util;

namespace gipo.core
{
	/// diffuse, absorbのコア部分
	/// メモリ管理があいまいになるとまずいのでIDisposableを持つ
	public class Diffuser : IDisposable {
		/// このdiffuserを持つインスタンス（何か
		private object holder;
		/// 親のdiffuser
		private Diffuser parent;
		/// クラス名:インスタンスの辞書
		private Dictionary<string, object> instanceClassDictionary;
		/// IDispose用フラグ
		private bool disposed = false;

		/// コンストラクタ
		public Diffuser(object holder) {
			this.holder = holder;
			parent = null;
			instanceClassDictionary = new Dictionary<string, object>();
		}

		/// 親のdiffuserの登録
		public void setParent(Diffuser parent) {
			this.parent = parent;
		}

		/// absorb用にインスタンスを登録する
		public void add(object diffuseInstance, Type clazz) {
			string className = clazz.ToString();
			if (instanceClassDictionary.ContainsKey(className)) {
				throw new Exception(string.Format("既に登録されているクラス{0}を登録しようとしました", className));
			}
			instanceClassDictionary[className] = diffuseInstance;
		}

		/// 登録されたインスタンスを削除（ほぼ使わない）
		public void remove(Type clazz) {
			string className = clazz.ToString();
			instanceClassDictionary.Remove(className);
		}

		/// クラス名で指定したインスタンスを取得（初期ステップ）
		public T get<T>(PosInfos pos) {
			Type clazz = typeof(T);
			string className = clazz.ToString();
			return getWithClassName<T>(className, this, pos);
		}

		/// クラス名で指定したインスタンスを取得（親にさかのぼりながら探索する）
		private T getWithClassName<T>(string className, Diffuser startDiffuser, PosInfos pos) {
			if (instanceClassDictionary.ContainsKey(className)) {
				/// 自分がそのクラスのインスタンスを保持していれば返す
				return (T)instanceClassDictionary[className];
			} else {
				/// ないので親に聞いてみる
				if (parent == null) {
					/// 親なしなので見つからない
					throw new Exception(string.Format("指定されたクラス{0}は{1}のDiffuserに登録されていません。;pos={2}", className, startDiffuser.holder, pos));
				}
				return parent.getWithClassName<T>(className, startDiffuser, pos);
			}
		}

		// for Debug
		public string DILog() {
			string ret = "";
			foreach (KeyValuePair<string, object> kv in instanceClassDictionary) {
				ret += " - " + kv.Key + "\n";
			}
			ret += "\n";
			return ret;
		}

		// IDisposable method
		~Diffuser() {
			this.Dispose(false);
		}

		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing) {
			if (!this.disposed) {
				if (isDisposing) {
					holder = null;
					parent = null;
					instanceClassDictionary.Clear();
				}
				this.disposed = true;
			}
		}
	}
}
