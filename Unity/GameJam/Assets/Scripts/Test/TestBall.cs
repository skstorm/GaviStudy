using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using GameJam;

public class TestBall : PoolObject
{
    int count;

    public override void Init()
    {
        base.Init();
        count = 0;
    }

    private void Update()
    {
        if(_isUpdateOk == false)
        {
            return;
        }

        count++;
        if(count > 60)
        {
            Hide();
        }
    }
}
