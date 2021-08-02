using UnityEngine;

namespace Fsm
{
    /// <summary>
    /// 遷移（Move）
    /// </summary>
    public class TestTransitionToMove : Transition<TestFsm, IState<TestFsm>>
    {
        /// <summary> コンストラクタ </summary>
        public TestTransitionToMove(TestFsm _owner, IState<TestFsm> nextState) : base(_owner, nextState)
        {

        }

        /// <summary> 遷移チェック </summary>
        public override bool Check()
        {
            _owner.Counting();
            if (_owner.StateCount > 20)
            {
                _owner.SetStateCount(0);
                return true;
            }
            return false;
        }

        /// <summary> 開放 </summary>
        public override void Release()
        {
            base.Release();
            Debug.Log("Release TestTransitionToMove");
        }
    }

    /// <summary>
    /// 遷移（Attack）
    /// </summary>
    public class TestTransitionToAttack : Transition<TestFsm, IState<TestFsm>>
    {
        /// <summary> コンストラクタ </summary>
        public TestTransitionToAttack(TestFsm _owner, IState<TestFsm> nextState) : base(_owner, nextState)
        {

        }

        /// <summary> 遷移チェック </summary>
        public override bool Check()
        {
            _owner.Counting();
            if (_owner.StateCount > 10)
            {
                _owner.SetStateCount(0);
                return true;
            }
            return false;
        }

        /// <summary> 開放 </summary>
        public override void Release()
        {
            base.Release();
            Debug.Log("Release TestTransitionToAttack");
        }
    }
}