using UnityEngine;
using System.Collections;

public class CoroutineFSM<T> : MonoBehaviour
{
	protected T m_state;

	protected bool m_isNewState = false;

	protected IEnumerator FSM()
	{
		while (true)
		{
			m_isNewState = false;
			yield return StartCoroutine(m_state.ToString());
		}
	}

	public void ChangeState(T state)
	{
		m_isNewState = true;
		m_state = state;
	}

	public T GetState()
	{
		return m_state;
	}
}
