namespace GameJam
{
    public class GameFsm : BaseFsm
    {
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            Localize.LanguageKind = ELanguageKind.En;
            var startState = BaseStateBehaviour.Load<TitleSceneState>("Prefabs/TitleScene", this);
            await fsm(startState);
        }
    }
}