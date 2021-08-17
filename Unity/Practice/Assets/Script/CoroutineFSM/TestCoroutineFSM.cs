using UnityEngine;
using System.Collections;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using Cysharp.Threading.Tasks.CompilerServices;
using Cysharp.Threading.Tasks.Linq;

public class TestCoroutineFSM : CoroutineFSM<TestCoroutineFSM.EState>
{
	public enum EState
	{
		Wait = 0,
		Move,
		Attack
	}
	
	// Use this for initialization
	void Awake()
	{
		m_state = EState.Wait;
		StartCoroutine(FSM());
	}
	
	IEnumerator Wait()
	{
		int count = 0;
		do
		{
			yield return null;
			Debug.Log("Wait State "+count);
			++count;
			if(count > 10)
			{
				ChangeState(EState.Attack);
			}
		} while (!m_isNewState);
	}

	IEnumerator Attack()
	{
		int count = 0;
		do
		{
			yield return null;
			Debug.Log("Attack State" + count);
			++count;
			if (count > 10)
			{
				ChangeState(EState.Wait);
			}
		} while (!m_isNewState);
	}
}
