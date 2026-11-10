using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ViewFinderManager : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject cinemachineCamera; // Đã thêm: Tắt Cinemachine để không bị kẹt góc nhìn
    [SerializeField] private Camera viewfinderCamera;

    [Header("UI System")]
    [SerializeField] private GameObject isometricUI;
    [SerializeField] private GameObject viewfinderUI;
    [SerializeField] private Button cameraButton;
    [SerializeField] private Button exitButton; // ĐÃ THÊM: Biến chứa nút Exit

    private PlayerInput playerInput;

    private void Start()
    {
        // Tự động tìm nhân vật
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
        }

        // Gắn sự kiện cho nút vào máy ảnh
        if (cameraButton != null)
        {
            cameraButton.onClick.AddListener(EnterViewfinderMode);
        }

        // ĐÃ THÊM: Gắn sự kiện cho nút Thoát (Exit)
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(ExitViewfinderMode);
        }

        viewfinderCamera.gameObject.SetActive(false);
        viewfinderUI.SetActive(false);
    }

    public void EnterViewfinderMode()
    {
        if (playerInput != null) playerInput.enabled = false;

        // 1. ĐỒNG BỘ CAMERA
        viewfinderCamera.transform.position = mainCamera.transform.position;
        viewfinderCamera.orthographicSize = mainCamera.orthographicSize;

        // 2. Tắt/Bật các thành phần
        mainCamera.gameObject.SetActive(false);
        if (cinemachineCamera != null) cinemachineCamera.SetActive(false);
        viewfinderCamera.gameObject.SetActive(true);

        // 3. ÉP TÍNH LẠI BỨC TƯỜNG GIỚI HẠN SAU KHI ĐÃ COPY SIZE
        // (Dòng này ngăn chặn việc camera bị giật ngược về không gian to)
        ViewfinderController controller = viewfinderUI.GetComponent<ViewfinderController>();
        if (controller != null)
        {
            controller.CalculateBounds();
        }

        isometricUI.SetActive(false);
        viewfinderUI.SetActive(true);
    }

    public void ExitViewfinderMode()
    {
        if (playerInput != null) playerInput.enabled = true;

        mainCamera.gameObject.SetActive(true);
        if (cinemachineCamera != null) cinemachineCamera.SetActive(true); // Bật Cinemachine
        viewfinderCamera.gameObject.SetActive(false);

        isometricUI.SetActive(true);
        viewfinderUI.SetActive(false);
    }
}