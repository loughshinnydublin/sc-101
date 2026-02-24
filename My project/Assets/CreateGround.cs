using UnityEngine;

public static class CreateGround
{

	// 生成 1x1 纯色 Sprite（世界尺寸通过 SpriteRenderer.size 控制）
	public static Sprite CreateSolidColorSprite(Color color)
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
			groundSprite = CreateSolidColorSprite(Color.green);
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
