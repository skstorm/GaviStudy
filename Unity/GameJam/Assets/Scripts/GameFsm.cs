using UnityEngine;

namespace GameJam
{
    public class GameFsm : BaseFsm
    {
        private async void Awake()
        {
            Localize.LanguageKind = ELanguageKind.En;
            Application.targetFrameRate = 60;

            DontDestroyOnLoad(this);

            var startState = Util.LoadScenePrefab<TitleSceneState>(Const.PathTitleScenePrefab, this);
            await fsm(startState);
        }
    }
}