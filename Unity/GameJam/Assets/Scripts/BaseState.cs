using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState
{
    protected IBaseFsm _ownerFsm;

    public BaseState(IBaseFsm fsm)
    {
        _ownerFsm = fsm;
    }

    public async UniTask Run()
    {
        enterState();
        do
        {
            await UniTask.DelayFrame(1);
            updateState();

        } while (!_ownerFsm.IsNewState);
        exitState();
    }

    protected virtual void enterState() { }
    protected virtual void updateState() { }
    protected virtual void exitState() { }
}
