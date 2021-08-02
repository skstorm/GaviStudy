namespace Fsm
{
    /// <summary>
    /// 状態マシン
    /// </summary>
    public class StateMachine<TOwner, TState> 
        where TOwner : class, new() 
        where TState : IState<TOwner>
    {
        /// <summary> 状態マシンを持っているオーナー </summary>
        protected TOwner _owner = default(TOwner);
        /// <summary> 現在状態 </summary>
        protected IState<TOwner> _currentState = null;
        /// <summary> 前の状態 </summary>
        protected IState<TOwner> _previousState = null;
        /// <summary> グローバル状態 </summary>
        protected IState<TOwner> _globalState = null;

        /// <summary> コンストラクタ</summary>
        public StateMachine(TOwner owner, IState<TOwner> currentState, IState<TOwner> globalState = null)
        {
            _owner = owner;
            _currentState = currentState;
            _globalState = globalState;
        }

        /// <summary> 更新 </summary>
        public void Update()
        {
            if (_globalState != null)
            {
                _globalState.Update(_owner);
            }

            if (_currentState != null)
            {
                if (_currentState.CheckTransition())
                {
                    var nextState = _currentState.NextTransition.NextState;
                    ChangeState(nextState);
                }
                else
                {
                    _currentState.Update(_owner);
                }
            }
        }

        /// <summary> 状態変更 </summary>
        public void ChangeState(IState<TOwner> newState)
        {
            _previousState = _currentState;

            _currentState.Exit(_owner);

            _currentState = newState;

            _currentState.Enter(_owner);
        }

        /// <summary> 状態戻し </summary>
        public void RevertToPreviousState()
        {
            ChangeState(_previousState);
        }

        /// <summary> 状態変更（同じ状態なら変更しない） </summary>
        public void ChangeState_Check(IState<TOwner> newState) 
        {
            if (newState == _currentState)
            {
                return;
            }

            ChangeState(newState);
        }
    }
}