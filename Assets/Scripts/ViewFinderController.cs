using UnityEngine;
using UnityEngine.EventSystems;

public class ViewfinderController : MonoBehaviour, IDragHandler
{
    [Header("Tâm Ngắm UI")]
    [SerializeField] private RectTransform crosshairRect;

    [Header("Camera & Di Chuyển")]
    [SerializeField] private Camera viewfinderCamera;
    [SerializeField] private float dragSpeed = 0.01f; // Độ nhạy vuốt ngón tay (có thể chỉnh lại)

    [Header("Giới Hạn Tự Động")]
    [SerializeField] private Collider2D confinerCollider;

    private float minX, maxX, minY, maxY;
    private bool hasBounds = false;

    private void Start()
    {
        // Khóa tâm ngắm ở giữa màn hình
        if (crosshairRect != null) crosshairRect.anchoredPosition = Vector2.zero;
    }

    // Tự động tính toán để không cần ViewFinderManager phải gọi
    private void CalculateBounds()
    {
        if (confinerCollider != null && viewfinderCamera != null)
        {
            Bounds bounds = confinerCollider.bounds;
            float camHeight = viewfinderCamera.orthographicSize;
            float camWidth = camHeight * viewfinderCamera.aspect;

            minX = bounds.min.x + camWidth;
            maxX = bounds.max.x - camWidth;
            minY = bounds.min.y + camHeight;
            maxY = bounds.max.y - camHeight;
            hasBounds = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (viewfinderCamera == null) return;

        // Ép tự tính lại ranh giới trước khi di chuyển
        CalculateBounds();

        Vector3 camPos = viewfinderCamera.transform.position;

        // Bỏ Time.deltaTime vì eventData.delta đã tự đo khoảng cách vuốt rất chuẩn
        camPos.x -= eventData.delta.x * dragSpeed;
        camPos.y -= eventData.delta.y * dragSpeed;

        if (hasBounds)
        {
            camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
            camPos.y = Mathf.Clamp(camPos.y, minY, maxY);
        }

        viewfinderCamera.transform.position = camPos;
    }
}