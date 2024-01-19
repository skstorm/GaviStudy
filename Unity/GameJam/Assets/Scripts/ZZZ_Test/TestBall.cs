using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using GameJam;

public class TestBall : Entity
{
    int count;

    public override void Init()
    {
        base.Init();
        count = 0;
        transform.localPosition = Vector3.zero;
    }

    public override void UpdateEntity()
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
