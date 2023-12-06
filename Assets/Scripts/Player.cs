using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private BoxCollider2D collLeg;
    private Animator anim;

    private Vector2 moveDir;

    [SerializeField] private bool isGround;
    private float verticalVelocity = 0.0f;
    [SerializeField] private float groundRatio = 0.1f;

    private bool isJump;
    private float gravity = 9.81f;
    private float fallingLimit = -10.0f;

    private bool isDash = false;
    private float dashTimer = 0.0f;
    [SerializeField] private float dashTime = 0.2f;



    [Header("Player 스탯")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpSpeed = 5.0f;
    [SerializeField] private float dashSpeed = 10.0f;

    [SerializeField] private bool isWallJump = false;//내가 벽을 찰수 있는지
    private bool doWallJump = false;

    private bool doWallJumpTimer = false;
    private float wallJumpTimer = 0.0f;
    private float wallJumpTime = 0.3f;



    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        moving();
        jumping();
        doAnim();

        checkDash();
        checkGround();
        checkGravity();
        checkDoJumpWallTimer();
    }

    private void moving()
    {
        if (doWallJumpTimer == true || isDash == true)
        {
            return;
        }

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

    private void jumping()
    {
        if (isGround == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) && isWallJump == true && moveDir.x != 0)
            {
                doWallJump = true;
            }
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }        
    }
    private void doAnim()
    {
        anim.SetInteger("VelocityX", (int)moveDir.x);

        if (isJump == true)
        {
            anim.SetBool("Jump", true);
        }
        else if (isJump == false)
        {
            anim.SetBool("Fall", true);
        }
    }
    private void checkGround()
    {
        isGround = false;

        if (verticalVelocity <= 0f)
        {
            RaycastHit2D hit = Physics2D.BoxCast(collLeg.bounds.center, collLeg.bounds.size, 0f, Vector2.down, groundRatio, LayerMask.GetMask("Ground"));

            if(hit)
            {
                isGround = true;
            }
        }
    }

    private void checkGravity()
    {
        if (isDash == true)
        {
            return;
        }

        if (doWallJump == true)
        {
            doWallJump = false;

            Vector2 dir = rigid.velocity;
            dir.x *= -1;
            rigid.velocity = dir;

            verticalVelocity = jumpSpeed;

            doWallJumpTimer = true;
        }

        else if (isGround == false)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            if (verticalVelocity < fallingLimit)
            {
                verticalVelocity = fallingLimit;
            }
        }
        else
        {
            if (isJump == true)
            {
                isJump = false;
                verticalVelocity = jumpSpeed;
            }
            else
            {
                verticalVelocity = 0.0f;
            }
        }
        rigid.velocity = new Vector2(rigid.velocity.x, verticalVelocity);
    }

    private void checkDash()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && isDash == false)
        {
            isDash = true;
            verticalVelocity = 0.0f;
            rigid.velocity = new Vector2(dashSpeed * transform.localScale.x, 0.0f);
        }
        else if (isDash == true)
        {
            dashTimer += Time.deltaTime;
            if (dashTimer >= dashTime)
            {
                dashTimer = 0.0f;
                isDash = false;
            }
        }
    }

    private void checkDoJumpWallTimer()
    {
        if (doWallJumpTimer == true)
        {
            wallJumpTimer += Time.deltaTime;
            if (wallJumpTimer >= wallJumpTime)
            {
                wallJumpTimer = 0.0f;
                doWallJumpTimer = false;
            }
        }
    }

    public void TriggerEnter(HitType _type, Collider2D _coll)
    {
        switch (_type)
        {
            case HitType.WallCheck:
                isWallJump = true;
                break;
            case HitType.ItemCheck:                
                break;
        }
    }

    public void TriggerExit(HitType _type, Collider2D _coll)
    {
        switch (_type)
        {
            case HitType.WallCheck:
                isWallJump = false;
                break;
            case HitType.ItemCheck:
                break;
        }
    }
}
