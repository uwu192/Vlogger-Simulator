using TMPro;
using UnityEngine;

public class PuzzleResultUI : MonoBehaviour
{
    public static PuzzleResultUI Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject fragmentHolderObject; // GameObject chứa FragmentHolder
    [SerializeField] private GameObject catalystHolderObject; // GameObject chứa CatalystHolder
    [SerializeField] private GameObject resultPanel;          // Panel chứa kết quả (Xem/Tiền)

    [Header("Text Displays (TextMeshPro)")]
    [SerializeField] private TextMeshProUGUI viewsText;  // UI hiển thị số lượt view
    [SerializeField] private TextMeshProUGUI moneyText;  // UI hiển thị số tiền nhận được

    private void Awake()
    {
        // Đảm bảo Instance được khởi tạo đúng
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Đảm bảo resultPanel ẩn khi bắt đầu
        if (resultPanel != null)
        {
            resultPanel.SetActive(false);
        }
    }

    // Hiển thị kết quả sau khi nhấn hoàn thành
    public void DisplayResults(int totalViews, int earnedMoney)
    {
        // 1. Ẩn 2 Holder
        if (fragmentHolderObject != null) fragmentHolderObject.SetActive(false);
        if (catalystHolderObject != null) catalystHolderObject.SetActive(false);

        // 2. Cập nhật số liệu UI
        if (viewsText != null) viewsText.text = $"Lượt view: {totalViews:N0}";
        if (moneyText != null) moneyText.text = $"Tiền nhận: ${earnedMoney:N0}";

        // 3. Hiện Panel kết quả
        if (resultPanel != null)
        {
            resultPanel.SetActive(true);
        }

        Debug.Log($"[PuzzleResultUI] Kết quả - Views: {totalViews} | Money: ${earnedMoney}");
    }
}