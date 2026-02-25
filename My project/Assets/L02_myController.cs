using UnityEngine;
using UnityEngine.InputSystem;

public class L02_myController : MonoBehaviour
{

    private float input_x;
    private float speed = 5f;
    private Rigidbody2D rb;
    private GameObject sq;


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
    }

    void Update()
    {
        GetInput();
        Move();
        Jump();
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

        print("input_x: " + input_x);
    }

    private void Move()
    {
        rb.linearVelocityX = input_x * speed;
    }

    private void Jump()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
        {
            print("Jump!");
            rb.AddForce(new Vector2(0f, 600f));
        }


    }
}