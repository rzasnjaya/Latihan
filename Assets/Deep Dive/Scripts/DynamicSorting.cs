using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void LateUpdate()
    {
        if (spriteRenderer != null)
        {
            // Update sorting order berdasarkan posisi Y
            // Makin ke bawah (Y lebih kecil) = sorting order lebih tinggi (di depan)
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
        }
    }
}