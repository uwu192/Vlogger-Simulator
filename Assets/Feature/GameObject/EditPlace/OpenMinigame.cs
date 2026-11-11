using UnityEngine;
using UnityEngine.InputSystem;

public class OpenMinigame : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject minigameUI;

    [Header("Mobile Settings")]
    [SerializeField] private GameObject interactButton; // Kéo nút bấm Tương tác trên màn hình vào đây

    private bool isPlayerInside = false;

    private void Start()
    {
        // Đảm bảo nút cảm ứng bị ẩn đi khi vừa vào game
        if (interactButton != null) interactButton.SetActive(false);
    }

    private void Update()
    {
        // Vẫn giữ phím E để ông bạn của bạn test trên PC cho nhàn
        if (isPlayerInside && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleMinigame();
        }
    }

    // HÀM DÀNH RIÊNG CHO MOBILE: Sẽ được gọi khi người chơi bấm nút trên màn hình
    public void OnMobileInteractTapped()
    {
        if (isPlayerInside)
        {
            ToggleMinigame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;

            // Player lại gần -> Bật nút cảm ứng lên cho họ bấm
            if (interactButton != null) interactButton.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            // Player đi ra xa -> Giấu nút cảm ứng đi
            if (interactButton != null) interactButton.SetActive(false);

            if (minigameUI != null)
            {
                minigameUI.SetActive(false);
            }
        }
    }

    private void ToggleMinigame()
    {
        if (minigameUI != null)
        {
            bool currentState = minigameUI.activeSelf;
            minigameUI.SetActive(!currentState);
        }
    }
}