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
    [SerializeField] private float sensorRadius = 200f;
    [SerializeField] private float maxScale = 4f;
    [SerializeField] private float minSpinSpeed = 20f;
    [SerializeField] private float maxSpinSpeed = 350f;

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
            float camWidth = camHeight * viewfinderCamera.aspect; // Đã sửa lỗi thiếu dấu *

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
        // Thêm crosshairImage vào đây. 
        // Nếu Camera, Crosshair Rect hoặc Crosshair Image bị xóa khi chuyển Scene, Update sẽ dừng ngay lập tức.
        if (viewfinderCamera == null || crosshairRect == null || crosshairImage == null) return;

        Vector2 centerPoint = viewfinderCamera.transform.position;
        Collider2D exactHit = Physics2D.OverlapPoint(centerPoint, poiLayer);

        if (exactHit != null)
        {
            // TRÚNG LÕI
            if (currentTargetPOI != exactHit.gameObject)
            {
                currentTargetPOI = exactHit.gameObject;
                crosshairImage.color = poiDetectedColor; // Không lo bị MissingReference nữa
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

                float scaleVal = Mathf.Lerp(maxScale, 1f, intensity);
                crosshairRect.localScale = new Vector3(scaleVal, scaleVal, 1f);

                float spinSpeed = Mathf.Lerp(minSpinSpeed, maxSpinSpeed, intensity);
                crosshairRect.Rotate(0, 0, spinSpeed * Time.deltaTime);

                Color hintColor = normalColor;
                hintColor.a = Mathf.Lerp(0f, 1f, intensity);
                crosshairImage.color = hintColor;
            }
            else
            {
                // NGOÀI VÙNG
                crosshairRect.localScale = Vector3.one;
                Color hiddenColor = normalColor;
                hiddenColor.a = 0f;
                crosshairImage.color = hiddenColor;
            }
        }
    }

    private void CapturePOI()
    {
        if (currentTargetPOI != null)
        {
            // Lấy component POI từ vật thể đang ngắm
            POI poiComponent = currentTargetPOI.GetComponent<POI>();

            if (poiComponent != null && poiComponent.fragmentData != null)
            {
                // Thêm mảnh vào PlayerInventory
                FragmentInventory.Instance.AddFragment(poiComponent.fragmentData);
                Debug.Log($"📸 Chụp thành công: {currentTargetPOI.name} -> Đã lưu mảnh '{poiComponent.fragmentData.fragmentName}' vào kho!");
            }
            else
            {
                Debug.LogWarning($"📸 Chụp {currentTargetPOI.name} nhưng không tìm thấy Component POI hoặc FragmentData!");
            }

            // Tắt vật thể POI trên Scene
            currentTargetPOI.SetActive(false);
            currentTargetPOI = null;

            if (captureButton != null) captureButton.interactable = false;

            if (crosshairRect != null) crosshairRect.localScale = Vector3.one;
            if (crosshairImage != null)
            {
                Color hiddenColor = normalColor;
                hiddenColor.a = 0f;
                crosshairImage.color = hiddenColor;
            }
        }
    }
}