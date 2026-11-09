using UnityEngine;
using UnityEngine.UI;
using TMPro; // Xóa dòng này và đổi TextMeshProUGUI thành Text nếu bạn dùng Text cũ của Unity

public class ItemInvenUI : MonoBehaviour
{
    [Header("UI Components")]
    public Image itemIcon;
    public TextMeshProUGUI nameText;

    private ItemData currentItem;

    // Hàm này sẽ được gọi khi bạn tạo ra ô vật phẩm này trong kho đồ
    public void SetupItem(ItemData itemData)
    {
        currentItem = itemData;

        if (itemData != null)
        {
            // Hiển thị hình ảnh
            if (itemIcon != null && itemData.icon != null)
            {
                itemIcon.sprite = itemData.icon;
            }

            // Hiển thị tên vật phẩm
            if (nameText != null)
            {
                nameText.text = itemData.itemName;
            }
        }
    }
}