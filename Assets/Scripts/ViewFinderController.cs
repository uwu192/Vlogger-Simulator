using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ViewfinderController : MonoBehaviour, IDragHandler
{
    [Header("Tâm Ngắm UI")]
    [SerializeField] private RectTransform crosshairRect;
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color poiDetectedColor = Color.green;

    [Header("Dò sóng & Hội tụ (Focus Lock-in)")]
    [SerializeField] private float sensorRadius = 200f; // Vùng phát hiện
    [SerializeField] private float maxScale = 4f; // Kích thước vòng ngắm khi ở xa
    [SerializeField] private float minSpinSpeed = 20f; // Tốc độ xoay chậm khi ở ngoài rìa
    [SerializeField] private float maxSpinSpeed = 350f; // Tốc độ xoay tít khi sát lõi

    [Header("Camera & Di Chuyển")]
    [SerializeField] private Camera viewfinderCamera;
    [SerializeField] private float dragSpeed = 0.01f;

    [Header("Hệ thống POI")]
    [SerializeField] private LayerMask poiLayer;
    [SerializeField] private Button captureButton;

    [Header("Giới Hạn Tự Động")]
    [SerializeField] private Collider2D confinerCollider;

    private float minX, maxX, minY, maxY;
    private bool hasBounds = false;
    private GameObject currentTargetPOI;

    private void Start()
    {
        if (crosshairRect != null) crosshairRect.anchoredPosition = Vector2.zero;

        if (captureButton != null)
        {
            captureButton.onClick.AddListener(CapturePOI);
            captureButton.interactable = false;
        }
    }

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
        CalculateBounds();

        Vector3 camPos = viewfinderCamera.transform.position;
        camPos.x -= eventData.delta.x * dragSpeed;
        camPos.y -= eventData.delta.y * dragSpeed;

        if (hasBounds)
        {
            camPos.x = Mathf.Clamp(camPos.x, minX, maxX);
            camPos.y = Mathf.Clamp(camPos.y, minY, maxY);
        }
        viewfinderCamera.transform.position = camPos;
    }

    private void Update()
    {
        if (viewfinderCamera == null || crosshairRect == null) return;

        Vector2 centerPoint = viewfinderCamera.transform.position;
        Collider2D exactHit = Physics2D.OverlapPoint(centerPoint, poiLayer);

        if (exactHit != null)
        {
            // TRÚNG LÕI
            if (currentTargetPOI != exactHit.gameObject)
            {
                currentTargetPOI = exactHit.gameObject;
                if (crosshairImage != null) crosshairImage.color = poiDetectedColor;
                if (captureButton != null) captureButton.interactable = true;
            }
            crosshairRect.localScale = Vector3.one;
            crosshairRect.localRotation = Quaternion.identity;
        }
        else
        {
            // ĐANG DÒ TÌM
            if (currentTargetPOI != null)
            {
                currentTargetPOI = null;
                if (captureButton != null) captureButton.interactable = false;
            }

            Collider2D nearbyHit = Physics2D.OverlapCircle(centerPoint, sensorRadius, poiLayer);
            if (nearbyHit != null)
            {
                Vector2 targetCenter = nearbyHit.bounds.center;
                float distance = Vector2.Distance(centerPoint, targetCenter);
                float intensity = 1f - Mathf.Clamp01(distance / sensorRadius);

                // Toán học lấy từ biến Inspector
                float scaleVal = Mathf.Lerp(maxScale, 1f, intensity);
                crosshairRect.localScale = new Vector3(scaleVal, scaleVal, 1f);

                float spinSpeed = Mathf.Lerp(minSpinSpeed, maxSpinSpeed, intensity);
                crosshairRect.Rotate(0, 0, spinSpeed * Time.deltaTime);

                if (crosshairImage != null)
                {
                    Color hintColor = normalColor;
                    hintColor.a = Mathf.Lerp(0f, 1f, intensity);
                    crosshairImage.color = hintColor;
                }
            }
            else
            {
                // NGOÀI VÙNG
                crosshairRect.localScale = Vector3.one;
                if (crosshairImage != null)
                {
                    Color hiddenColor = normalColor;
                    hiddenColor.a = 0f;
                    crosshairImage.color = hiddenColor;
                }
            }
        }
    }

    private void CapturePOI()
    {
        if (currentTargetPOI != null)
        {
            Debug.Log("📸 Đã chụp thành công: " + currentTargetPOI.name);

            // 1. Tắt vật thể POI đi (coi như đã thu thập xong)
            currentTargetPOI.SetActive(false);
            // Nếu bạn muốn xóa hẳn nó khỏi bộ nhớ thì dùng: Destroy(currentTargetPOI);

            // 2. Xóa trí nhớ của hệ thống ngắm
            currentTargetPOI = null;

            // 3. Tắt nút chụp
            if (captureButton != null) captureButton.interactable = false;

            // 4. Ép tâm ngắm tàng hình ngay lập tức (tránh bị kẹt bóng mờ)
            if (crosshairRect != null) crosshairRect.localScale = Vector3.one;
            if (crosshairImage != null)
            {
                Color hiddenColor = normalColor;
                hiddenColor.a = 0f; // Trả alpha về 0
                crosshairImage.color = hiddenColor;
            }

            // GỢI Ý MỞ RỘNG: Chỗ này có thể gọi hàm phát âm thanh "Tách!", 
            // chớp trắng màn hình, hoặc cộng điểm cho nhân vật.
        }
    }
}