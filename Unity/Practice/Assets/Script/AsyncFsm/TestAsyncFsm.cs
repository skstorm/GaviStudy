using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

public class TestAsyncFsm : AsyncFsm<TestAsyncFsm.EState>
{
    public enum EState
    {
        Wait = 0,
        Move,
        Attack
    }

    // Use this for initialization
    async UniTask Awake()
    {
        _state = EState.Wait;
        //		await StartCoroutine(FSM());
        // await FSM().ToUniTask();
        await Fsm();
    }

    protected override Func<UniTask> getActionAsync()
    {
        Func<UniTask> actionAsync = null;
        switch (_state)
        {
            case EState.Wait: actionAsync = Wait; break;
            case EState.Attack: actionAsync = Attack; break;
        }

        return actionAsync;
    }

    async UniTask Wait()
    {
        int count = 0;
        do
        {
            await UniTask.Yield();
            Debug.Log("Wait State " + count);
            ++count;
            if (count > 10)
            {
                ChangeState(EState.Attack);
            }
        } while (!_isNewState);
    }

    async UniTask Attack()
    {
        int count = 0;
        do
        {
            await UniTask.Yield();
            Debug.Log("Attack State" + count);
            ++count;
            if (count > 10)
            {
                ChangeState(EState.Wait);
            }
        } while (!_isNewState);
    }
}
