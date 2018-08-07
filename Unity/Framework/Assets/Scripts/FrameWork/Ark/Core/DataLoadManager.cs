using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ark.Core
{
	public class DataLoadManager : MonoBehaviour
	{
		private string _bundleUrl = "";

		private WWW _www = null;

		private int _version = 1;

		private Dictionary<string, GameObject> _dicAssets = new Dictionary<string, GameObject>();

		public void Init()
		{
			StartCoroutine(LoadAssetBundle());
		}

		private IEnumerator LoadAssetBundle()
		{
			while (!Caching.ready)
			{
				yield return null;
			}

			_www = WWW.LoadFromCacheOrDownload(_bundleUrl, _version);
			yield return _www;

			if (_www.error != null)
			{
				throw new Exception("WWW ダウンロードにエラーが発生しました。：" + _www.error);
			}

			AssetBundle bundle = _www.assetBundle;

			for (int i = 0; i < 3; i++)
			{
				AssetBundleRequest request = bundle.LoadAssetAsync("TestAb" + (i + 1), typeof(GameObject));
				yield return request;

				GameObject obj = Instantiate(request.asset) as GameObject;
				obj.transform.position = new Vector3(-10.0f + (i * 10), 0.0f, 0.0f);
				obj.transform.rotation = Quaternion.identity;
			}
			bundle.Unload(false);
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

		public void Release()
		{
			_www.Dispose();
			_www = null;

			_dicAssets.Clear();
			_dicAssets = null;
		}
	}
}