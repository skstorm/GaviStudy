using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStateA : BaseState
{
    private int count = 0;
    public TestStateA(IBaseFsm fsm) : base(fsm)
    {
    }

    protected override void enterState() 
    {
        Debug.Log("A Start");
    }
    protected override void updateState() 
    {
        ++count;
        Debug.Log("A Update" + count);
        if (count > 10)
        {
            _ownerFsm.ChangeState(new TestStateB(_ownerFsm));
        }
    }
    protected override void exitState() 
    {
        Debug.Log("A End");
    }
}

public class TestStateB : BaseState
{
    private int count = 0;
    public TestStateB(IBaseFsm fsm) : base(fsm)
    {
    }

    protected override void enterState()
    {
        Debug.Log("B Start");
    }
    protected override void updateState()
    {
        ++count;
        Debug.Log("B Update" + count);
        if (count > 10)
        {
            _ownerFsm.ChangeState(new TestStateA(_ownerFsm));
        }
    }
    protected override void exitState()
    {
        Debug.Log("B End");
    }
}
