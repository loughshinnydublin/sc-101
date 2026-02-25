using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine.Rendering;

// 物理系统，简单平台跳跃逻辑

public class L02 : MonoBehaviour
{
    private readonly List<CircleCollider2D> circleColliders = new List<CircleCollider2D>();

    void Start()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("MainCamera not found.");
            return;
        }
        Vector2 groundSize = new Vector2(50f, 1f);
        GameObject ground = CreateGround.Build(cam, groundSize, 4f, 1f);
        SpriteRenderer sr = ground.GetComponent<SpriteRenderer>();
        sr.color = Color.grey;
        GameObject plank = CreateGround.Build(cam, new Vector2(5f, 1f), 1f, 1f);
        sr = plank.GetComponent<SpriteRenderer>();
        sr.color = Color.yellow;


        for (int i = 0; i < 3; i++)
        {
            GameObject go = new("Circle" + i);
            sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = CreateGround.CreateCircleSprite(64);
            // sr.color不是覆盖色
            sr.color = Color.red;
            go.transform.position = new Vector3(0f, i * 2f, 1f);
            CircleCollider2D circleCollider = sr.AddComponent<CircleCollider2D>();
            circleColliders.Add(circleCollider);
            sr.AddComponent<Rigidbody2D>();
        }

        StartCoroutine(DisableCircleCollidersAfterDelay(5f));
    }

    //协程函数，5s后禁用所有CircleCollider2D组件
    private IEnumerator DisableCircleCollidersAfterDelay(float delaySeconds)
    {
        // 等待指定的秒数
        yield return new WaitForSeconds(delaySeconds);

        for (int i = 0; i < circleColliders.Count; i++)
        {
            CircleCollider2D circleCollider = circleColliders[i];
            if (circleCollider != null)
            {
                circleCollider.enabled = false;
            }
        }
    }

    


}
