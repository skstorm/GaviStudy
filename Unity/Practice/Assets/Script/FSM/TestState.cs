using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GaviPractice.FSM
{
	/// <summary>
	/// 基底状態
	/// </summary>
	public abstract class TestState<TInterface, TState> : State<TestFSM, TState> where TState : class, new() where TInterface : ITestFSM_Base
	{
		public sealed override void MainEnter(TestFSM entity)
		{
			ITestFSM_Base baseEntity = entity;
			Enter((TInterface)baseEntity);
		}

		public sealed override void MainUpdate(TestFSM entity)
		{
			ITestFSM_Base baseEntity = entity;
			Update((TInterface)baseEntity);
		}
		public sealed override void MainExit(TestFSM entity)
		{
			ITestFSM_Base baseEntity = entity;
			Exit((TInterface)baseEntity);
		}

		public abstract void Enter(TInterface entity);

		public abstract void Update(TInterface entity);

		public abstract void Exit(TInterface entity);
	}

	/// <summary>
	/// 移動状態
	/// </summary>
	public class TestMoveState : TestState<ITestFSM_ForMove, TestMoveState>
	{
		private int m_count;

		public override void Enter(ITestFSM_ForMove entity)
		{
			m_count = 0;
			Debug.Log("enter move");
		}
		public override void Update(ITestFSM_ForMove entity)
		{
			++m_count;
			Debug.Log("update move " + m_count);
			if (m_count > 10)
			{
				entity.ChangeState(TestAttackState.GetInstance());
			}
		}
		public override void Exit(ITestFSM_ForMove entity)
		{
			Debug.Log("Exit move");
		}
	}

	/// <summary>
	/// 攻撃状態
	/// </summary>
	public class TestAttackState : TestState<ITestFSM_ForAttack, TestAttackState>
	{
		private int m_count;

		public override void Enter(ITestFSM_ForAttack entity)
		{
			m_count = 0;
			Debug.Log("enter attack");
		}
		public override void Update(ITestFSM_ForAttack entity)
		{
			++m_count;
			Debug.Log("update attack" + m_count);
			if (m_count > 10)
			{
				entity.ChangeState(TestMoveState.GetInstance());
			}
		}
		public override void Exit(ITestFSM_ForAttack entity)
		{
			Debug.Log("Exit attack");
		}
	}
}