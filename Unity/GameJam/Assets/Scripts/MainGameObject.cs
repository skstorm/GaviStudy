using UnityEngine;

namespace GameJam
{
    public class MainGameObject : MonoBehaviour
    {
        private async void Awake()
        {
            Localize.LanguageKind = ELanguageKind.En;
            Application.targetFrameRate = 60;

            DontDestroyOnLoad(this);

            var gameFsm = new BaseFsm();

            var startState = Util.LoadScenePrefab<TitleSceneState>(Const.PathTitleScenePrefab, gameFsm);
            await gameFsm.RunAsync(startState);
            gameFsm.Release();
        }
    }
}