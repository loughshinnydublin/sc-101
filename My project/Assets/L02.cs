using Unity.VisualScripting;
using UnityEngine;

// 物理系统，简单平台跳跃逻辑

public class L02 : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("MainCamera not found.");
            return;
        }
        Vector2 groundSize = new Vector2(50f, 1f);
        float offsetBelowCamera = 4f;
        float cameraForwardOffset = 1f;
        CreateGround.Build(cam, groundSize, offsetBelowCamera, cameraForwardOffset);

        for (int i = 0; i < 3; i++)
        {
            GameObject go = new("Circle" + i);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = CreateGround.CreateCircleSprite(Color.white, 64);
            // sr.color不是覆盖色
            // sr.color = Color.red;
            go.transform.position = new Vector3(0f, i * 2f, 1f);
            sr.AddComponent<CircleCollider2D>();
            sr.AddComponent<Rigidbody2D>();

        }
    }

    void Update()
    {

    }


}
