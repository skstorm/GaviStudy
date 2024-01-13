using Ark.Core;
using System;
using UnityEditor.PackageManager.UI;
using UnityEngine.Scripting;

namespace Example
{
    public class MySetting : ISetting
    {
        private readonly IBaseSceneLogic _startScene;
        public IBaseSceneLogic StartScene => _startScene;

        public int Fps => 60;

        private readonly uint _displayLogLevel = 0;
        public uint DisplayLogLevel => _displayLogLevel;

        //private readonly string _bundleUrl = "file:///D:/Work/Project/GaviStudy/Unity/Framework/Assets/AssetBundles/test_asset_bundle";
        private readonly string _bundleUrl = "file:///E:/Study/GaviStudy/Unity/Framework/Assets/AssetBundles/test_asset_bundle";

        public string BundleUrl => _bundleUrl;


        public IBaseSceneLogic CreateStartScene()
        {
            return _startSceneName switch
            {
                "MenuSceneLogic" => new MenuSceneLogic(),
                "BattleSceneLogic" => new BattleSceneLogic(),
                _ => null,
            };
        }

        private string _startSceneName;


        public MySetting(string startSceneName)
        {
            _startSceneName = startSceneName;
            _displayLogLevel = ArkLogLevelDefine.Error | ArkLogLevelDefine.Warning | ArkLogLevelDefine.Info | ArkLogLevelDefine.Debug;
        }
    }
}