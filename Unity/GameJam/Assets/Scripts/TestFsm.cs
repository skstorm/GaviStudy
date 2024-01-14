public class TestFsm : BaseFsm
{
    private async void Awake()
    {
        DontDestroyOnLoad(this);
        await fsm(new TestStateA(this));
    }
}
