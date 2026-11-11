using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Bắt buộc phải có để điều khiển trạng thái Button

public class TravelBoard : MonoBehaviour
{
    [Header("Giao diện Bảng Di Chuyển")]
    [SerializeField] private GameObject travelUI;
    [SerializeField] private GameObject interactButton;

    [Header("Hệ thống Khóa Địa Danh")]
    // Khai báo các nút hình ảnh địa danh
    [SerializeField] private Button locationA_Button; // VD: Khu phố A
    [SerializeField] private bool isLocationA_Unlocked = true; // Mặc định mở

    [SerializeField] private Button locationB_Button; // VD: Nhà ga
    [SerializeField] private bool isLocationB_Unlocked = false; // Mặc định khóa

    private bool isPlayerInside = false;

    private void Start()
    {
        if (travelUI != null) travelUI.SetActive(false);
        if (interactButton != null) interactButton.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInside && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            OpenTravelBoard();
        }
    }

    public void OnMobileInteractTapped()
    {
        if (isPlayerInside)
        {
            OpenTravelBoard();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            if (interactButton != null && (travelUI == null || !travelUI.activeSelf))
            {
                interactButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            if (interactButton != null) interactButton.SetActive(false);
            CloseTravelBoard();
        }
    }

    public void OpenTravelBoard()
    {
        if (travelUI != null)
        {
            travelUI.SetActive(true);

            // --- KÍCH HOẠT TÍNH NĂNG KHÓA KHI VỪA MỞ BẢNG ---
            if (locationA_Button != null)
                locationA_Button.interactable = isLocationA_Unlocked;

            if (locationB_Button != null)
                locationB_Button.interactable = isLocationB_Unlocked;
            // ------------------------------------------------

            if (interactButton != null) interactButton.SetActive(false);
        }
    }

    public void CloseTravelBoard()
    {
        if (travelUI != null)
        {
            travelUI.SetActive(false);
            if (isPlayerInside && interactButton != null)
            {
                interactButton.SetActive(true);
            }
        }
    }

    public void TeleportTo(string sceneName)
    {
        Debug.Log("Bắt đầu dịch chuyển đến: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}