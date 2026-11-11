using TMPro; // Sử dụng TextMeshPro (hoặc "using UnityEngine.UI;" nếu dùng Text cũ)
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickUpFragment : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText; // Kéo UI Text số lượng vào đây

    public FragmentData fragmentData { get; private set; }
    public CatalystData catalystData { get; private set; }

    public int Count { get; private set; } = 1; // Số lượng mảnh hiện tại (Mặc định = 1)
    public bool IsCatalyst => catalystData != null;
    public Sprite Sprite => image != null ? image.sprite : null;

    private void Awake()
    {
        if (image == null) image = GetComponent<Image>();
    }

    public void SetupFragment(FragmentData data, int initialCount = 1)
    {
        fragmentData = data;
        catalystData = null;
        Count = initialCount;

        if (image == null) image = GetComponent<Image>();
        if (image != null && fragmentData != null)
        {
            image.sprite = fragmentData.GetImage();
        }

        UpdateCountUI();
    }

    public void SetupCatalyst(CatalystData data, int initialCount = 3)
    {
        catalystData = data;
        fragmentData = null;
        Count = initialCount;

        if (image == null) image = GetComponent<Image>();
        if (image != null && catalystData != null)
        {
            image.sprite = catalystData.icon;
        }

        UpdateCountUI();
    }

    // Tăng/Giảm số lượng mảnh
    public void AddCount(int amount)
    {
        Count += amount;
        if (Count < 0) Count = 0;
        UpdateCountUI();
    }

    // Cập nhật số hiển thị lên UI
    private void UpdateCountUI()
    {
        if (amountText != null)
        {
            // Nếu > 1 thì hiện số lượng, nếu = 1 (Fragment thường) hoặc = 0 thì ẩn/hiện tùy ý
            amountText.text = Count > 0 ? Count.ToString() : "0";
            amountText.gameObject.SetActive(Count > 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Chỉ cho phép chọn nếu số lượng > 0
        if (Count <= 0)
        {
            Debug.LogWarning($"[PickUpFragment] Loại mảnh này đã hết số lượng (Count = 0)!");
            return;
        }

        PuzzleManager.Instance.Select(this);
        Debug.Log($"[PickUpFragment] Chọn mảnh {catalystData?.catalystType} | Còn lại: {Count}");
    }
}