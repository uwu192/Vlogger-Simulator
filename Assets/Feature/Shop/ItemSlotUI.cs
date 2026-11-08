using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI priceText;

    private Button slotButton; // Nút bấm của Item Frame
    private ItemData currentItem;

    private void Awake()
    {
        // Lấy component Button và gán sự kiện khi click
        slotButton = GetComponent<Button>();
        if (slotButton != null)
        {
            slotButton.onClick.AddListener(OnSlotClicked);
        }
    }

    public void SetupSlot(ItemData itemAsset)
    {
        currentItem = itemAsset;
        if (itemAsset.icon != null) itemIcon.sprite = itemAsset.icon;
        if (priceText != null) priceText.text = itemAsset.price.ToString() + " VNĐ";
    }

    // Hàm chạy khi người chơi bấm vào ô vật phẩm này
    private void OnSlotClicked()
    {
        // Kiểm tra xem bảng hiển thị có tồn tại không và gửi dữ liệu sang
        if (currentItem != null && ItemInfoDisplayUI.Instance != null)
        {
            ItemInfoDisplayUI.Instance.ShowItemInfo(currentItem);
        }
    }
}