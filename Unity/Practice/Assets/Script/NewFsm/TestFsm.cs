using UnityEngine;

namespace Fsm
{
    /// <summary>
    /// テスト状態マシン
    /// </summary>
    public class TestFsm : MonoBehaviour, ITestFsm_ForMove, ITestFsm_ForAttack
    {
        /// <summary> 状態マシン </summary>
        private StateMachine<TestFsm, IState<TestFsm>> _stateMachine;

        /// <summary> 状態カウント </summary>
        private int _stateCount = 0;
        /// <summary> 状態カウント </summary>
        public int StateCount { get { return _stateCount; } }
        /// <summary> 状態カウント </summary>
        public void Counting()
        {
            ++_stateCount;
        }
        /// <summary> 状態カウントセット </summary>
        public void SetStateCount(int _count)
        {
            _stateCount = _count;
        }


        /// <summary> 初期化 </summary>
        private void Awake()
        {
            // 状態初期化
            var moveState = TestMoveState.CreateInstance();
            var attackState = TestAttackState.CreateInstance();

            // 遷移設定
            var transToAttack = new TestTransitionToAttack(this, attackState);
            moveState.AddTransition(transToAttack);
            var transToMove = new TestTransitionToMove(this, moveState);
            attackState.AddTransition(transToMove);

            // 状態マシン初期化
            _stateMachine = new StateMachine<TestFsm, IState<TestFsm>>(this, moveState);
            _stateMachine.ChangeState(moveState);
        }

        /// <summary> 更新 </summary>
        private void Update()
        {
            _stateMachine.Update();
        }

        /// <summary> 開放 </summary>
        private void OnDestroy()
        {
            TestMoveState.ReleaseInstance();
            TestAttackState.ReleaseInstance();
        }
    }

    /// <summary>
    /// 状態マシンのInterface（共通）
    /// </summary>
    public interface ITestFsm_Base
    {

    }

    /// <summary>
    /// 状態マシンのInterface（移動用）
    /// </summary>
    public interface ITestFsm_ForMove : ITestFsm_Base
    {
        int StateCount { get; }
    }

    /// <summary>
    /// 状態マシンのInterface（攻撃用）
    /// </summary>
    public interface ITestFsm_ForAttack : ITestFsm_Base
    {
        int StateCount { get; }
    }
}