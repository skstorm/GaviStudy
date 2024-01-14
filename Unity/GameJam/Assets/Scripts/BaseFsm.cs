using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBaseFsm
{
    bool IsNewState { get; }
    void ChangeState(BaseState state);
}

public class BaseFsm : MonoBehaviour, IBaseFsm
{
    protected BaseState _state;

    protected bool _isNewState = false;
    public bool IsNewState => _isNewState;

    protected async UniTask fsm(BaseState startState)
    {
        _state = startState;

        while (true)
        {
            _isNewState = false;
            await _state.Run();
        }
    }

    public void ChangeState(BaseState state)
    {
        _isNewState = true;
        _state = state;
    }

    public BaseState GetState()
    {
        return _state;
    }
}
