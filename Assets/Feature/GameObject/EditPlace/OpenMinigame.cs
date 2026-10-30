using UnityEngine;
using UnityEngine.InputSystem;

public class OpenMinigame : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject minigameUI; // Kéo Canvas/Panel Minigame vào đây


    private bool isPlayerInside = false;

    private void Update()
    {
        // Chỉ nhận phím bấm khi Player đang đứng trong vùng tương tác
        if (isPlayerInside && Keyboard.current.eKey.wasPressedThisFrame)
        {
            ToggleMinigame();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;

            // Tùy chọn: Tự động đóng minigame nếu Player đi ra xa
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
            // Bật/tắt minigame UI
            bool currentState = minigameUI.activeSelf;
            minigameUI.SetActive(!currentState);
        }
    }
}