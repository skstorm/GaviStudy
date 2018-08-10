using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ark.Core
{
	public class DataLoadManager : MonoBehaviour
	{
		private WWW _www = null;

		private int _version = 1;

		private Dictionary<string, GameObject> _dicAssets = new Dictionary<string, GameObject>();

		public void Init(string bundleUrl)
		{
			StartCoroutine(LoadAssetBundle(bundleUrl));
		}

		private IEnumerator LoadAssetBundle(string bundleUrl)
		{
			while (!Caching.ready)
			{
				yield return null;
			}

			_www = WWW.LoadFromCacheOrDownload(bundleUrl, _version);
			yield return _www;

			if (_www.error != null)
			{
				throw new Exception("WWW ダウンロードにエラーが発生しました。：" + _www.error);
			}
		}

		public void LoadAllAsset()
		{
			AssetBundle bundle = _www.assetBundle;
			GameObject[] assetsObjs = bundle.LoadAllAssets<GameObject>();

			for(int i=0; i<assetsObjs.Length; ++i)
			{
				GameObject obj = assetsObjs[i];
				_dicAssets.Add(obj.name, obj);
			}
		}

		public GameObject InstantiateAsset(string name)
		{
			return Instantiate(_dicAssets[name]) as GameObject;
		}

		public GameObject GetAsset(string name)
		{
			return _dicAssets[name];
		}

		public void Release()
		{
			_www.Dispose();
			_www = null;

			_dicAssets.Clear();
			_dicAssets = null;
		}
	}
}