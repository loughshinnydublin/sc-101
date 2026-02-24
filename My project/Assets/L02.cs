using System.Collections;
using UnityEngine;

public class L02 : MonoBehaviour
{
	// 可在 Inspector 手动指定；不指定时自动在场景中查找 L01
	public L01 l01;

	private GameObject ground;

	private IEnumerator Start()
	{
		if (l01 == null)
		{
			l01 = FindFirstObjectByType<L01>();
		}

		if (l01 == null)
		{
			Debug.LogError("L02: Scene 中找不到 L01 组件。");
			yield break;
		}

		// L01 在 Start 里创建 ground，这里等待直到创建完成
		while (l01.ground == null)
		{
			yield return null;
		}

		ground = l01.ground;

		// 示例：把当前物体放到 ground 上方 1 个单位
		transform.position = ground.transform.position + Vector3.up;

		Debug.Log($"L02: 已获取 ground -> {ground.name}"); 
	}
}
