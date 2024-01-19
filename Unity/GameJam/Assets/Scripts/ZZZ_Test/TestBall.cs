using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using GameJam;

public class TestBall : Entity
{
    int count;

    [SerializeField]
    private int _life;

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
        if(count > _life)
        {
            IsWillRemove = true;
            //Release();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Util.DebugLog("ball trigger enter");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Util.DebugLog("ball collision enter");
        }
    }
}
