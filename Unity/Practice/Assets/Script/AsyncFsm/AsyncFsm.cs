using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System;


public abstract class AsyncFsm<T> : MonoBehaviour
{
    protected T _state;

    protected bool _isNewState = false;

    protected async UniTask Fsm()
    {
        while (true)
        {
            _isNewState = false;
            var action = getActionAsync();
            await action();
        }
    }

    protected abstract Func<UniTask> getActionAsync();

    public void ChangeState(T state)
    {
        _isNewState = true;
        _state = state;
    }

    public T GetState()
    {
        return _state;
    }
}
