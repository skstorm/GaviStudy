using System;
using System.Collections.Generic;
using gipo.util;

namespace gipo.core
{
	/// diffuse, absorbのコア部分
	/// メモリ管理があいまいになるとまずいのでIDisposableを持つ
	public class Diffuser : IDisposable
	{
		/// このdiffuserを持つインスタンス（何か
		private object _holder;
		/// 親のdiffuser
		private Diffuser _parent;
		/// クラス名:インスタンスの辞書
		private Dictionary<string, object> _instanceClassDictionary;
		/// IDispose用フラグ
		private bool _disposed = false;

		/// コンストラクタ
		public Diffuser(object holder)
		{
			_holder = holder;
			_parent = null;
			_instanceClassDictionary = new Dictionary<string, object>();
		}

		/// 親のdiffuserの登録
		public void SetParent(Diffuser parent)
		{
			_parent = parent;
		}

		/// absorb用にインスタンスを登録する
		public void Add(object diffuseInstance, Type clazz)
		{
			string className = clazz.ToString();
			if (_instanceClassDictionary.ContainsKey(className))
			{
				throw new Exception(string.Format("既に登録されているクラス{0}を登録しようとしました", className));
			}
			_instanceClassDictionary[className] = diffuseInstance;
		}

		/// 登録されたインスタンスを削除（ほぼ使わない）
		public void Remove(Type clazz)
		{
			string className = clazz.ToString();
			_instanceClassDictionary.Remove(className);
		}

		/// クラス名で指定したインスタンスを取得（初期ステップ）
		public T Get<T>(PosInfos pos)
		{
			Type clazz = typeof(T);
			string className = clazz.ToString();
			return GetWithClassName<T>(className, this, pos);
		}

		/// クラス名で指定したインスタンスを取得（親にさかのぼりながら探索する）
		private T GetWithClassName<T>(string className, Diffuser startDiffuser, PosInfos pos)
		{
			if (_instanceClassDictionary.ContainsKey(className))
			{
				/// 自分がそのクラスのインスタンスを保持していれば返す
				return (T)_instanceClassDictionary[className];
			}
			else
			{
				/// ないので親に聞いてみる
				if (_parent == null)
				{
					/// 親なしなので見つからない
					throw new Exception(string.Format("指定されたクラス{0}は{1}のDiffuserに登録されていません。;pos={2}", className, startDiffuser._holder, pos));
				}
				return _parent.GetWithClassName<T>(className, startDiffuser, pos);
			}
		}

		// for Debug
		public string DILog()
		{
			string ret = "";
			foreach (KeyValuePair<string, object> kv in _instanceClassDictionary)
			{
				ret += " - " + kv.Key + "\n";
			}
			ret += "\n";
			return ret;
		}

		// IDisposable method
		~Diffuser()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool isDisposing)
		{
			if (!_disposed)
			{
				if (isDisposing)
				{
					_holder = null;
					_parent = null;
					_instanceClassDictionary.Clear();
				}
				_disposed = true;
			}
		}
	}
}