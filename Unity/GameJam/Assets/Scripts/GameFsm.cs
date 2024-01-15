namespace GameJam
{
    public class GameFsm : BaseFsm
    {
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            Localize.LanguageKind = ELanguageKind.En;
            var startState = Util.LoadScenePrefab<TitleSceneState>(Const.PathTitleScenePrefab, this);
            await fsm(startState);
        }
    }
}