using UnityEngine;

public static class CreateGround
{

	// 生成 1x1 纯色 Sprite（世界尺寸通过 SpriteRenderer.size 控制）
	public static Sprite CreateSolidColorSprite()
	{
		Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false)
		{
			filterMode = FilterMode.Point,
			wrapMode = TextureWrapMode.Clamp
		};

		texture.SetPixel(0, 0, Color.white);
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

	public static Sprite CreateCircleSprite(int diameter = 64)
	{
		diameter = Mathf.Max(2, diameter);	//最小直径为2


		// 正方形多余部分会被透明色覆盖
		Texture2D texture = new Texture2D(diameter, diameter, TextureFormat.RGBA32, false)
		{
			filterMode = FilterMode.Bilinear,
			wrapMode = TextureWrapMode.Clamp
		};

		Color transparent = new Color(0f, 0f, 0f, 0f);
		float radius = (diameter - 1) * 0.5f;
		Vector2 center = new Vector2(radius, radius);
		float radiusSqr = radius * radius;

		// 遍历每个像素，判断是否在圆内
		for (int y = 0; y < diameter; y++)
		{
			for (int x = 0; x < diameter; x++)
			{
				float dx = x - center.x;
				float dy = y - center.y;
				float distSqr = dx * dx + dy * dy;

				texture.SetPixel(x, y, distSqr <= radiusSqr ? Color.white : transparent);
			}
		}

		texture.Apply();

		return Sprite.Create(
			texture,
			new Rect(0f, 0f, diameter, diameter),
			new Vector2(0.5f, 0.5f),
			diameter,
			0,
			SpriteMeshType.FullRect
		);
	}

	public static GameObject Build(
		Camera cam,
		Vector2 groundSize,
		float offsetBelowCamera,
		float cameraForwardOffset,
		Sprite groundSprite = null)
	{
		if (cam == null)
		{
			Debug.LogError("CreateGround: MainCamera not found.");
			return null;
		}

		GameObject ground = new GameObject("Ground");
		SpriteRenderer sp = ground.AddComponent<SpriteRenderer>();

		if (groundSprite == null)
		{
			groundSprite = CreateSolidColorSprite();
		}

		sp.sprite = groundSprite;
		sp.color = Color.white;
		sp.drawMode = SpriteDrawMode.Sliced;
		sp.size = groundSize;

		BoxCollider2D groundCollider = ground.AddComponent<BoxCollider2D>();
		groundCollider.size = groundSize;

		Vector3 camPos = cam.transform.position;
		float zPos = camPos.z + cameraForwardOffset;
		ground.transform.position = new Vector3(camPos.x, camPos.y - offsetBelowCamera, zPos);
		ground.transform.localScale = Vector3.one;

		return ground;
	}

}
