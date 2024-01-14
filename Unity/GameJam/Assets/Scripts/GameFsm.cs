namespace GameJam
{
    public class GameFsm : BaseFsm
    {
        private async void Awake()
        {
            DontDestroyOnLoad(this);
            await fsm(new TitleSceneState(this));
        }
    }
}