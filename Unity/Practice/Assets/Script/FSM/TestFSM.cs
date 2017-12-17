using UnityEngine;
using System.Collections;

public class TestFSM : MonoBehaviour
{
	public StateMachine<TestFSM, IState<TestFSM> > m_stateMachine;

	// Use this for initialization
	void Awake()
	{
		m_stateMachine = new StateMachine<TestFSM, IState<TestFSM>>(this, TestMoveState.GetInstance());
		m_stateMachine.ChangeState(TestMoveState.GetInstance());
	}
	
	// Update is called once per frame
	void Update ()
	{
		m_stateMachine.Update();
	}
}

public class TestMoveState : State<TestFSM, TestMoveState>
{
	private int m_count;
	
	public override void Enter(TestFSM entity)
	{
		m_count = 0;
		Debug.Log("enter move");
	}
	public override void Update(TestFSM entity)
	{
		++m_count;
		Debug.Log("update move "+m_count);
		if (m_count > 10)
		{
			entity.m_stateMachine.ChangeState(TestAttackState.GetInstance());
		}
	}
	public override void Exit(TestFSM entity)
	{
		Debug.Log("enter move");
	}
}

public class TestAttackState : State<TestFSM, TestAttackState>
{
	private int m_count;
	
	public override void Enter(TestFSM entity)
	{
		m_count = 0;
		Debug.Log("enter attack");
	}
	public override void Update(TestFSM entity)
	{
		++m_count;
		Debug.Log("update attack" + m_count);
		if (m_count > 10)
		{
			entity.m_stateMachine.ChangeState(TestMoveState.GetInstance());
		}
	}
	public override void Exit(TestFSM entity)
	{
		Debug.Log("enter attack");
	}
}