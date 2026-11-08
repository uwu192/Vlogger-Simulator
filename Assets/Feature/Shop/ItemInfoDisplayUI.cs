using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoDisplayUI : MonoBehaviour
{
    // Sử dụng Singleton để các ItemFrame dễ dàng gọi bảng này từ bất kỳ đâu
    public static ItemInfoDisplayUI Instance { get; private set; }

    [Header("--- THÔNG TIN CHUNG ---")]
    public Image itemImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    [Header("--- THÔNG SỐ CAMERA ---")]
    public TextMeshProUGUI resolutionText;      // Nối với ResolutionBonus
    public TextMeshProUGUI viewMultiplierText;  // Nối với ViewMultiplier (TMP)

    private ItemData currentSelectedItem;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Tắt bảng thông tin khi mới mở game (chưa chọn đồ)
        gameObject.SetActive(false);
    }

    // Hàm này sẽ được gọi khi người chơi bấm vào 1 ItemFrame
    public void ShowItemInfo(ItemData item)
    {
        currentSelectedItem = item;
        gameObject.SetActive(true); // Bật UI hiển thị lên

        // 1. Cập nhật thông tin chung
        itemImage.sprite = item.icon;
        nameText.text = "Tên: " + item.itemName;
        priceText.text = "Giá tiền: " + item.price.ToString();

        // 2. Kiểm tra loại vật phẩm
        if (item is CameraData cameraData)
        {
            // Nếu là Camera: Bật 2 dòng thông số và gán giá trị
            resolutionText.gameObject.SetActive(true);
            viewMultiplierText.gameObject.SetActive(true);

            // Ghi chú: Đảm bảo biến trong CameraItemData của bạn khớp với chữ ở đây
            // Giả sử bạn đổi tên zoomDistance thành resolution, flashCooldown thành viewMultiplier
            resolutionText.text = "Độ phân giải: " + cameraData.resolutionBonus;
            viewMultiplierText.text = "Hệ số nhân view: " + cameraData.viewMultiplier;
        }
        else
        {
            // Nếu là Decor: Tắt hẳn 2 dòng thông số đi
            resolutionText.gameObject.SetActive(false);
            viewMultiplierText.gameObject.SetActive(false);
        }

        // 3. Gắn sự kiện cho nút Mua
        buyButton.onClick.RemoveAllListeners(); // Xóa sự kiện cũ của vật phẩm trước đó
        buyButton.onClick.AddListener(OnClickBuy);
    }

    private void OnClickBuy()
    {
        // Gọi tới ShopManager đã làm ở các bước trước
        if (currentSelectedItem != null && ShopManager.Instance != null)
        {
            ShopManager.Instance.TryPurchaseItem(currentSelectedItem);
        }
    }
}