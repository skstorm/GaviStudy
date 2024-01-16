using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameJam
{
    public interface IStateMachine
    {
        bool IsNewState { get; }
        void ChangeState(IBaseState state);
    }

    public class StateMachine : IStateMachine
    {
        protected IBaseState _state;

        protected bool _isNewState = false;
        public bool IsNewState => _isNewState;

        public async UniTask RunAsync(IBaseState startState)
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

        public void Release()
        {
            _state.Release();
        }
    }
}