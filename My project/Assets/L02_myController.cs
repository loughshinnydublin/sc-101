using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class L02_myController : MonoBehaviour
{

    private float input_x;
    private float speed = 5f;
    private float jumpImpulse = 12f;    //跳跃冲量
    private float jumpCutFactor = 0.5f; //松开跳跃键时保留的向上速度比例（0~1）
    private float dropThroughDuration = 0.01f;   //平台虚化时间
    private Rigidbody2D rb;
    private GameObject sq;

    //跳跃计数器，允许双跳，在玩家着陆时重置
    private int jumpCount = 0;
    private bool wasGrounded = true;
    private Collider2D col;
    private LayerMask platformMask;
    private bool isDropping;

    void Start()
    {
        //sq为可操作方块
        sq = new("Square");
        SpriteRenderer sr = sq.AddComponent<SpriteRenderer>();
        sr.sprite = CreateGround.CreateSolidColorSprite();
        sr.color = Color.cyan;
        sq.AddComponent<BoxCollider2D>();
        sq.AddComponent<Rigidbody2D>();
        sq.transform.position = new Vector3(-5f, 0f, 1f);
        rb = sq.GetComponent<Rigidbody2D>();    //不在update调用getcomponent
        rb.gravityScale = 2f;
        col = sq.GetComponent<Collider2D>();
        platformMask = LayerMask.GetMask("Platform");
    }

    void Update()
    {
        GetInput();
        Move();
        Jump();
        HandleJumpCut();
        Raycast();
    }

    private void Raycast()
    {
        //从square底边中心向下发射一条射线，长度为0.1f，检测是否与地面碰撞
        Vector3 origin = col.bounds.center + new Vector3(0f, -col.bounds.extents.y, 0f);

        //获取square底边的位置，使用collider
        int groundMask = LayerMask.GetMask("Ground", "Platform");
        if (isDropping)
        {
            groundMask = LayerMask.GetMask("Ground");
        }
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, groundMask);
        Debug.DrawLine(origin, origin + Vector3.down * 0.1f);
        bool hitGround = hit.collider != null;  //是否击中地面

        // 只在“落地瞬间”重置：上一帧不在地面，本帧在地面，且竖直速度不向上
        if (hitGround && !wasGrounded && rb.linearVelocity.y <= 0f)
        {
            jumpCount = 0; //重置跳跃计数器
            print("Grounded!");
        }

        wasGrounded = hitGround;
    }


    private void GetInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            input_x = 0f;
            return;
        }

        input_x = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            input_x -= 1f;
        }
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            input_x += 1f;
        }

        // print("input_x: " + input_x);
    }

    private void Move()
    {
        rb.linearVelocityX = input_x * speed;
    }

    private void Jump()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null || !keyboard.spaceKey.wasPressedThisFrame)
        {
            return;
        }

        bool pressDown = keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed;
        if (pressDown && TryStartDropThrough())
        {
            return;
        }

        if (jumpCount < 2)
        {
            print("Jump!");
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
            jumpCount++;
        }
    }

    private bool TryStartDropThrough()
    {
        if (isDropping || platformMask == 0)
        {
            return false;
        }

        Vector2 feetCenter = new Vector2(col.bounds.center.x, col.bounds.min.y - 0.02f);
        Vector2 checkSize = new Vector2(col.bounds.size.x * 0.9f, 0.05f);
        Collider2D platformCollider = Physics2D.OverlapBox(feetCenter, checkSize, 0f, platformMask);

        if (platformCollider == null)
        {
            return false;
        }

        StartCoroutine(DropThroughPlatform(platformCollider));
        return true;
    }

    private IEnumerator DropThroughPlatform(Collider2D platformCollider)
    {
        isDropping = true;
        Physics2D.IgnoreCollision(col, platformCollider, true);
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Min(rb.linearVelocity.y, -2f));

        yield return new WaitForSeconds(dropThroughDuration);


        //等待玩家完全穿过平台：当玩家底部低于平台顶部时，结束忽略碰撞
        while (platformCollider != null && col.bounds.min.y > platformCollider.bounds.max.y)
        {
            yield return null;
        }

        if (platformCollider != null)
        {
            Physics2D.IgnoreCollision(col, platformCollider, false);
        }

        isDropping = false;
    }

    private void HandleJumpCut()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        // 在上升阶段松开空格，提前截断上升速度，实现可变跳高
        if (keyboard.spaceKey.wasReleasedThisFrame && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutFactor);
        }
    }
}