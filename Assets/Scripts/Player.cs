using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    private Animator anim;

    private Vector2 moveDir;

    [Header("Player Ω∫≈»")]
    [SerializeField] private float moveSpeed = 5.0f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        moving();
        doAnim();
    }

    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        rigid.velocity = new Vector2(moveDir.x * moveSpeed, rigid.velocity.y);
        
        if (moveDir.x > 0f && transform.localScale.x != 1.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
        else if (moveDir.x < 0f && transform.localScale.x != -1.0f)
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        }
    }

    private void doAnim()
    {
        anim.SetInteger("VelocityX", (int)moveDir.x);
    }
}
