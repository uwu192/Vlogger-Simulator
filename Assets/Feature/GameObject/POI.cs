using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class POI : MonoBehaviour
{
    [Header("Dữ liệu Fragment nhận được khi chụp")]
    public FragmentData fragmentData;

    private void OnValidate()
    {
        // Tự động kiểm tra Collider2D để đảm bảo Physics2D Overlap hoạt động
        if (GetComponent<Collider2D>() == null)
        {
            Debug.LogWarning($"[POI] GameObject '{gameObject.name}' cần có Component Collider2D (Trigger hoặc Solid)!");
        }
    }
}