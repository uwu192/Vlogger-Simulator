using UnityEngine;
using UnityEngine.InputSystem;

public class ViewfinderController : MonoBehaviour
{
    [Header("Tâm Ngắm UI")]
    [SerializeField] private RectTransform crosshairRect;

    [Header("Camera & Di Chuyển")]
    [SerializeField] private Camera viewfinderCamera;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private float edgeThreshold = 50f;

    [Header("Giới Hạn Tự Động (Kéo CameraBouderies vào đây)")]
    [SerializeField] private Collider2D confinerCollider; // Tự động lấy ranh giới từ Collider này

    private float minX, maxX, minY, maxY;
    private bool hasBounds = false;

    private void Start()
    {
        CalculateBounds();
    }

    // Nếu đổi map khác hoặc đổi kích thước camera, có thể gọi lại hàm này
    public void CalculateBounds()
    {
        if (confinerCollider != null && viewfinderCamera != null)
        {
            // Lấy kích thước hình hộp bao quanh Collider
            Bounds bounds = confinerCollider.bounds;

            // Tính toán kích thước thực tế của Camera trên màn hình
            float camHeight = viewfinderCamera.orthographicSize;
            float camWidth = camHeight * viewfinderCamera.aspect;

            // Tính giới hạn tọa độ tâm Camera (Khóa mép Camera chạm mép Collider)
            minX = bounds.min.x + camWidth;
            maxX = bounds.max.x - camWidth;
            minY = bounds.min.y + camHeight;
            maxY = bounds.max.y - camHeight;

            hasBounds = true;
        }
        else
        {
            Debug.LogWarning("Chưa gắn Confiner Collider hoặc Viewfinder Camera!");
        }
    }

    private void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePos = Mouse.current.position.ReadValue();

        if (crosshairRect != null) crosshairRect.position = mousePos;

        if (viewfinderCamera != null)
        {
            Vector3 camPos = viewfinderCamera.transform.position;

            if (mousePos.x < edgeThreshold) camPos.x -= panSpeed * Time.deltaTime;
            else if (mousePos.x > Screen.width - edgeThreshold) camPos.x += panSpeed * Time.deltaTime;

            if (mousePos.y < edgeThreshold) camPos.y -= panSpeed * Time.deltaTime;
            else if (mousePos.y > Screen.height - edgeThreshold) camPos.y += panSpeed * Time.deltaTime;

            // Áp dụng giới hạn tự động nếu đã tính toán xong
            if (hasBounds)
            {
                camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
                camPos.y = Mathf.Clamp(camPos.y, minY, maxY);
            }

            viewfinderCamera.transform.position = camPos;
        }
    }

    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }
}