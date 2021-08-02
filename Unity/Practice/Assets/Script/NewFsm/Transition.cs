namespace Fsm
{
    /// <summary>
    /// 遷移クラス
    /// </summary>
    public abstract class Transition<TOwner, TState> 
        where TOwner : class, new()
        where TState : IState<TOwner>
    {
        /// <summary> 次の遷移 </summary>
        private TState _nextState = default(TState);
        /// <summary> 次の遷移 </summary>
        public TState NextState { get { return _nextState; } }

        /// <summary> 遷移を持っているオーナー </summary>
        protected TOwner _owner = default(TOwner);

        /// <summary> コンストラクタ </summary>
        public Transition(TOwner owner, TState nextState)
        {
            _owner = owner;
            _nextState = nextState;
        }

        /// <summary> 遷移チェック </summary>
        public abstract bool Check();

        /// <summary> 開放 </summary>
        public virtual void Release()
        {
            _owner = null;
        }
    }
}