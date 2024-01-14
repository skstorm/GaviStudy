public class TestFsm : BaseFsm
{
    private async void Awake()
    {
        await fsm(new TestStateA(this));
    }
}
