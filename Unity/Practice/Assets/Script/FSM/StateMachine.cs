using UnityEngine;
using System.Collections;

public class StateMachine<TOwner, TState> where TOwner : class where TState : IState<TOwner>
{
	protected TOwner m_owner;
	protected IState<TOwner> m_currentState = null;
	protected IState<TOwner> m_previousState = null;
	protected IState<TOwner> m_globalState = null;

	public IState<TOwner> CurrentState
	{
		get
		{
			return m_currentState;
		}
		set
		{
			m_currentState = value;
		}
	}

	public IState<TOwner> GlobalState
	{
		get
		{
			return m_globalState;
		}
		set
		{
			m_globalState = value;
		}
	}

	public StateMachine(TOwner owner, IState<TOwner> currentState, IState<TOwner> globalState = null)
	{
		m_owner = owner;
		m_currentState = currentState;
		m_globalState = globalState;
	}

	public void Update()
	{
		if (m_globalState != null)
		{
			m_globalState.Update(m_owner);
		}
		if (m_currentState!=null)
		{
			m_currentState.Update(m_owner);
		}
	}

	public void ChangeState(IState<TOwner> newState)
	{
		m_previousState = m_currentState;

		m_currentState.Exit(m_owner);

		m_currentState = newState;

		m_currentState.Enter(m_owner);
	}

	public void RevertToPreviousState()
	{
		ChangeState(m_previousState);
	}

	public void ChangeState_Another(IState<TOwner> newState) //변할려는 상태가 지금의 상태라면은 상태바꾸지 않음.
	{
		if (newState == m_currentState)
			return;

		ChangeState(newState);
	}
}
