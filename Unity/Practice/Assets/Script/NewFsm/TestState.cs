using UnityEngine;

namespace Fsm
{
    /// <summary>
    /// 基底状態
    /// </summary>
    public abstract class TestState<TInterface, TState> : State<TestFsm, TState> , ISingleton
        where TState : ISingleton, new() 
        where TInterface : ITestFsm_Base
    {
        /// <summary> 状態に入る時 </summary>
        public sealed override void Enter(TestFsm entity)
        {
            ITestFsm_Base baseEntity = entity;
            enter((TInterface)baseEntity);
        }

        /// <summary> 更新 </summary>
        public sealed override void Update(TestFsm entity)
        {
            ITestFsm_Base baseEntity = entity;
            update((TInterface)baseEntity);
        }

        /// <summary> 状態から出るとき </summary>
        public sealed override void Exit(TestFsm entity)
        {
            ITestFsm_Base baseEntity = entity;
            exit((TInterface)baseEntity);
        }

        /// <summary> 状態に入る時 </summary>
        protected abstract void enter(TInterface entity);

        /// <summary> 更新 </summary>
        protected abstract void update(TInterface entity);

        /// <summary> 状態から出るとき </summary>
        protected abstract void exit(TInterface entity);
    }

    /// <summary>
    /// 移動状態
    /// </summary>
    public class TestMoveState : TestState<ITestFsm_ForMove, TestMoveState>
    {
        /// <summary> 状態に入る時 </summary>
        protected override void enter(ITestFsm_ForMove entity)
        {
            Debug.Log("enter move");
        }

        /// <summary> 更新 </summary>
        protected override void update(ITestFsm_ForMove entity)
        {
            Debug.Log("update move " + entity.StateCount);
        }

        /// <summary> 状態から出る時 </summary>
        protected override void exit(ITestFsm_ForMove entity)
        {
            Debug.Log("Exit move");
        }
    }

    /// <summary>
    /// 攻撃状態
    /// </summary>
    public class TestAttackState : TestState<ITestFsm_ForAttack, TestAttackState>
    {
        /// <summary> 状態に入る時 </summary>
        protected override void enter(ITestFsm_ForAttack entity)
        {
            Debug.Log("enter attack");
        }

        /// <summary> 更新 </summary>
        protected override void update(ITestFsm_ForAttack entity)
        {
            Debug.Log("update attack" + entity.StateCount);
        }

        /// <summary> 状態から出るとき </summary>
        protected override void exit(ITestFsm_ForAttack entity)
        {
            Debug.Log("Exit attack");
        }
    }
}