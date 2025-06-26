using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class YSortOrder : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        // -100 * y座標（小さいほどSortingOrderが大きくなって手前に）
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
    }
}
