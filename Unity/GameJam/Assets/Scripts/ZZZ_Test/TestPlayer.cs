using GameJam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rigid;

    [SerializeField]
    private float _speed;

    [SerializeField]
    private float _jumpPower;

    [SerializeField]
    private Vector2 _kickPower;

    // Update is called once per frame
    void Update()
    {
        //var x = Input.GetAxisRaw("Horizontal");
        //var y = Input.GetAxisRaw("Jump") * _jumpPower;
        float x = 0;
        float y = 0;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            x = -_speed;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            x = _speed;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            y = _jumpPower;
        }

        var moveDir = new Vector3(x, y, 0);
        _rigid.velocity = moveDir * _speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ball")
        {
            Util.DebugLog("trigger enter");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Util.DebugLog("collision enter");
        if (collision.collider.tag == "Ball")
        {
            collision.rigidbody.AddForce(_kickPower, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Respawn")
        {
            Util.DebugLog("trigger stay");
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Respawn")
        {
            Util.DebugLog("collision stay");
        }
    }
}
