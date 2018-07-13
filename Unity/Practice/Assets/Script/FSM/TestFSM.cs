using UnityEngine;

namespace GaviPractice.FSM
{
	/// <summary>
	/// 状態マシンの基底Interface
	/// </summary>
	public interface ITestFSM_Base
	{

	}

	/// <summary>
	/// 状態マシンのInterface（移動用）
	/// </summary>
	public interface ITestFSM_ForMove : ITestFSM_Base
	{
		void ChangeState(IState<TestFSM> newState);
	}

	/// <summary>
	/// 状態マシンのInterface（攻撃用）
	/// </summary>
	public interface ITestFSM_ForAttack : ITestFSM_Base
	{
		void ChangeState(IState<TestFSM> newState);
	}

	/// <summary>
	/// テスト状態マシン
	/// </summary>
	public class TestFSM : MonoBehaviour, ITestFSM_ForMove, ITestFSM_ForAttack
	{
		public StateMachine<TestFSM, IState<TestFSM>> m_stateMachine;

		void Awake()
		{
			m_stateMachine = new StateMachine<TestFSM, IState<TestFSM>>(this, TestMoveState.GetInstance());
			m_stateMachine.ChangeState(TestMoveState.GetInstance());
		}

		public void ChangeState(IState<TestFSM> newState)
		{
			m_stateMachine.ChangeState(newState);
		}

		void Update()
		{
			m_stateMachine.Update();
		}
	}
}