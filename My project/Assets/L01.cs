using UnityEngine;

// 创建ground、一些基本语法、以及一个简单的定时器

public class L01 : MonoBehaviour
{
    // 运行时创建出的地面对象
    public GameObject ground;
    public Sprite groundSprite;
    // 地面尺寸（世界单位）
    // public会导致不能直接修改代码中的groundsize
    private Vector2 groundSize = new Vector2(50f, 1f);
    // 地面相对主相机向下偏移
    public float offsetBelowCamera = 4f;
    // 修改game中object位置，确保object在相机前面
    private float cameraForwardOffset = 1f;

    public GameObject square;

    private SpriteRenderer[] srs = new SpriteRenderer[10];

    void Start()
    {
        // 获取主相机并进行空值保护
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("MainCamera not found.");
            return;
        }

        // 这里只保留示例方块创建，地面创建交给 L02 调用独立类
        Vector3 camPos = cam.transform.position;
        float zPos = camPos.z + cameraForwardOffset;

        for (int i = 0; i < 10; i++)
        {
            GameObject go = new("Square" + i);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = CreateGround.CreateSolidColorSprite();
            sr.color = Color.blue;
            srs[i] = sr;
            go.transform.position = new Vector3(camPos.x - 7f + i * 1.5f, camPos.y, zPos);
            go.transform.localScale = Vector3.one;
        }

        ground = CreateGround.Build(cam, groundSize, offsetBelowCamera, cameraForwardOffset);
    }



    private float timer = 0f;
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            Debug.Log("1 second passed");
            timer = 0f;
            for (int i = 0; i < 10; i++)
            {
                srs[i].color = new Color(Random.value, Random.value, Random.value);
            }
        }



    }

    void FixedUpdate()
    {

    }
    void OnDestroy()
    {
        
    }
}
