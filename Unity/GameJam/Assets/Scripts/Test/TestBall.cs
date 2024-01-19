using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using GameJam;

public class TestBall : PoolObject, IEntity
{
    int count;

    public Vector2 Pos => Vector2.zero;

    public float Radius => 0;

    public bool IsWillRemove => false;

    public override void Init()
    {
        base.Init();
        count = 0;
        transform.localPosition = Vector3.zero;
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
