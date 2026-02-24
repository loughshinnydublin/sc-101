using UnityEngine;


// 创建ground、一些基本语法、以及一个简单的定时器

public class L01 : MonoBehaviour
{
    // 运行时创建出的地面对象
    public GameObject ground;
    // 地面的渲染器引用
    public SpriteRenderer sp;
    public Sprite groundSprite;
    // 地面尺寸（世界单位）
    // public会导致不能直接修改代码中的groundsize
    private Vector2 groundSize = new Vector2(50f, 1f);
    // 地面相对主相机向下偏移
    private float offsetBelowCamera = 4f;
    // 纯2d,相机设为-1
    private float cameraForwardOffset = -1f;

    public GameObject square;

    private SpriteRenderer[] srs = new SpriteRenderer[10];
    private BoxCollider2D groundCollider;


    // 生成 1x1 纯色 Sprite（世界尺寸通过 SpriteRenderer.size 控制）
    private static Sprite CreateSolidColorSprite(Color color)
    {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        texture.SetPixel(0, 0, color);
        texture.Apply();

        return Sprite.Create(
            texture,
            new Rect(0f, 0f, 1f, 1f),
            new Vector2(0.5f, 0.5f),
            1f,
            0,
            SpriteMeshType.FullRect
        );
    }

    // 同步地面的显示尺寸与碰撞体尺寸
    private void ApplyGroundSize()
    {
        if (sp != null)
        {
            sp.drawMode = SpriteDrawMode.Sliced;
            sp.size = groundSize;
        }

        if (groundCollider != null)
        {
            groundCollider.size = groundSize;
        }
    }

    void Start()
    {
        // 获取主相机并进行空值保护
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogError("MainCamera not found.");
            return;
        }

        // 创建地面对象与渲染组件
        ground = new GameObject("Ground");
        sp = ground.AddComponent<SpriteRenderer>();

        // 未手动指定贴图时，自动生成纯绿色默认贴图
        if (groundSprite == null)
        {
            groundSprite = CreateSolidColorSprite(Color.green);
        }

        // 使用 Sliced + size 明确控制地面为长方形
        sp.sprite = groundSprite;
        sp.color = Color.white;

        // 添加 2D 碰撞器并同步尺寸
        groundCollider = ground.AddComponent<BoxCollider2D>();
        ApplyGroundSize();

        // 放置到主相机下方，并确保在相机前方可见
        Vector3 camPos = cam.transform.position;
        float zPos = camPos.z + cameraForwardOffset;
        ground.transform.position = new Vector3(camPos.x, camPos.y - offsetBelowCamera, zPos);
        ground.transform.localScale = Vector3.one;


        for (int i = 0; i < 10; i++)
        {
            GameObject go = new("Square" + i);
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = CreateSolidColorSprite(Color.red);
            srs[i] = sr;
            go.transform.position = new Vector3(camPos.x - 7f + i * 1.5f, camPos.y, zPos);
            go.transform.localScale = Vector3.one;
        }
    }

    private void OnValidate()
    {
        ApplyGroundSize();
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
