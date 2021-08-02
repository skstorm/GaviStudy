using System.Collections.Generic;

namespace Fsm
{
    /// <summary>
    /// 状態Interface
    /// </summary>
    public interface IState<TOwner> 
        where TOwner : class, new()
    {
        /// <summary> 状態に入るとき </summary>
        void Enter(TOwner entity);
        /// <summary> 更新 </summary>
        void Update(TOwner entity);
        /// <summary> 状態から出るとき </summary>
        void Exit(TOwner entity);
        /// <summary> 遷移チェック </summary>
        bool CheckTransition();
        /// <summary> 次の遷移 </summary>
        Transition<TOwner, IState<TOwner>> NextTransition { get; }
        /// <summary> 遷移追加 </summary>
        void AddTransition(Transition<TOwner, IState<TOwner>> _transition);
    }

    /// <summary>
    /// 状態
    /// </summary>
    public abstract class State<TOwner, TState> : Singleton<TState>, IState<TOwner> 
        where TState : ISingleton, new()
        where TOwner : class, new() 
    {
        /// <summary> 遷移リスト </summary>
        private List<Transition<TOwner, IState<TOwner>>> _transitionList = null;
        /// <summary> 次の遷移 </summary>
        private Transition<TOwner, IState<TOwner> > _nextTransition = default(Transition<TOwner, IState<TOwner> >);
        /// <summary> 次の遷移 </summary>
        public Transition<TOwner, IState<TOwner> > NextTransition { get { return _nextTransition; } }

        /// <summary> 初期化 </summary>
        public override void Init()
        {
            _transitionList = new List<Transition<TOwner, IState<TOwner>>>();
        }

        /// <summary> 遷移追加 </summary>
        public void AddTransition(Transition<TOwner, IState<TOwner>> _transition)
        {
            _transitionList.Add(_transition);
        }

        /// <summary> 遷移チェック </summary>
        public bool CheckTransition()
        {
            for(int i=0; i< _transitionList.Count; ++i)
            {
                var transition = _transitionList[i];
                if(transition.Check())
                {
                    _nextTransition = transition;
                    return true;
                }
            }

            return false;
        }

        /// <summary> 開放 </summary>
        public override void Release()
        {
            for (int i = 0; i < _transitionList.Count; ++i)
            {
                var transition = _transitionList[i];
                transition.Release();
            }
        }

        /// <summary> 状態に入るとき </summary>
        public abstract void Enter(TOwner entity);
        /// <summary> 更新 </summary>
        public abstract void Update(TOwner entity);
        /// <summary> 状態から出るとき </summary>
        public abstract void Exit(TOwner entity);
    }
}