using UnityEngine;

public class L01 : MonoBehaviour
{
    // 运行时创建出的地面对象
    public GameObject ground;
    // 地面的渲染器引用
    public SpriteRenderer sp;
    // 可选：手动指定地面精灵；为空时自动生成纯绿色精灵
    public Sprite groundSprite;
    // 地面尺寸（世界单位）
    public Vector2 groundSize = new Vector2(10f, 1f);
    // 地面相对主相机向下偏移
    public float offsetBelowCamera = 4f;

    // 根据给定尺寸与颜色生成一张纯色 Sprite
    private static Sprite CreateSolidColorSprite(Color color, Vector2 size)
    {
        int width = Mathf.Max(1, Mathf.RoundToInt(size.x));
        int height = Mathf.Max(1, Mathf.RoundToInt(size.y));

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Point,
            wrapMode = TextureWrapMode.Clamp
        };

        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = color;
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 1f);
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
            groundSprite = CreateSolidColorSprite(Color.green, groundSize);
        }

        // 使用 Sliced + size 明确控制地面为长方形
        sp.sprite = groundSprite;
        sp.drawMode = SpriteDrawMode.Sliced;
        sp.size = groundSize;
        sp.color = Color.white;

        // 添加 2D 碰撞器并同步尺寸
        BoxCollider2D collider2D = ground.AddComponent<BoxCollider2D>();
        collider2D.size = groundSize;

        // 放置到主相机下方
        Vector3 camPos = cam.transform.position;
        ground.transform.position = new Vector3(camPos.x, camPos.y - offsetBelowCamera, 0f);
        ground.transform.localScale = Vector3.one;
    }
}
