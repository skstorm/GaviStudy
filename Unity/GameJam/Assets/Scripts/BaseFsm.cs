using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameJam
{
    public interface IBaseFsm
    {
        bool IsNewState { get; }
        void ChangeState(IBaseState state);
    }

    public class BaseFsm : MonoBehaviour, IBaseFsm
    {
        protected IBaseState _state;

        protected bool _isNewState = false;
        public bool IsNewState => _isNewState;

        protected async UniTask fsm(IBaseState startState)
        {
            _state = startState;

            while (true)
            {
                _isNewState = false;
                await _state.Run();
            }
        }

        public void ChangeState(IBaseState state)
        {
            _isNewState = true;
            _state = state;
        }
    }
}