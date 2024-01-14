namespace GameJam
{
    public class GameFsm : BaseFsm
    {
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            Localize.LanguageKind = ELanguageKind.En;
            await fsm(new TitleSceneState(this));
        }
    }
}