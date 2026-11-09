using System.Collections.Generic;
using UnityEngine;

public class ItemInvenDisplay : MonoBehaviour
{
    [Header("Dữ liệu & Cấu hình")]
    public ItemCatalog catalog;            // Kho dữ liệu tổng để dò tìm thông tin theo ID
    public GameObject itemInvenPrefab;     // Prefab của ItemInvenFrame (khung chứa UI kho đồ)
    public Transform contentTransform;     // Vị trí Content trong Scroll View của Inventory

    private void OnEnable()
    {
        // Khi mở bảng Inventory UI lên, tự động cập nhật danh sách món đồ sở hữu
        RefreshCameraInventory();
        // Đăng ký sự kiện: Khi mua đồ mới thành công trong Shop, túi đồ sẽ tự động làm mới
        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.OnItemPurchased += HandleItemPurchased;
        }
    }

    private void OnDisable()
    {
        // Phải hủy đăng ký sự kiện khi tắt bảng Inventory để tránh Memory Leak
        if (PlayerInventory.Instance != null)
        {
            PlayerInventory.Instance.OnItemPurchased -= HandleItemPurchased;
        }
    }

    // Hàm refresh danh sách Camera người chơi đang sở hữu
    public void RefreshCameraInventory()
    {
        ClearDisplay();

        if (PlayerInventory.Instance == null || catalog == null)
        {
            Debug.Log("Inventory dang trong");
            return;
        }
        Debug.Log($"[InventoryUI] Dang co {PlayerInventory.Instance.OwnedItems.Count} item trong kho.");
        // 1. Duyệt qua tất cả ID vật phẩm mà người chơi đã sở hữu
        foreach (string itemID in PlayerInventory.Instance.OwnedItems)
        {
            // 2. Tìm ItemData tương ứng trong Catalog
            ItemData itemData = catalog.allItems.Find(x => x.itemID == itemID);

            // 3. Lọc: Chỉ xử lý nếu vật phẩm tồn tại VÀ là loại CameraItemData
            if (itemData != null && itemData is CameraData)
            {
                // Sinh ra 1 khung ItemInvenFrame mới trong Content
                GameObject newFrame = Instantiate(itemInvenPrefab, contentTransform);

                // Gán dữ liệu sang script ItemInvenUI đã viết ở bước trước
                ItemInvenUI invenUI = newFrame.GetComponent<ItemInvenUI>();
                if (invenUI != null)
                {
                    invenUI.SetupItem(itemData);
                }
            }
        }
    }

    // Tự động làm mới khi mua đồ mới
    private void HandleItemPurchased(ItemData purchasedItem)
    {
        // Nếu vừa mua một chiếc Camera, cập nhật lại kho đồ ngay lập tức
        if (purchasedItem is CameraData)
        {
            RefreshCameraInventory();
        }
    }

    // Dọn dẹp sạch các ô cũ trước khi nạp dữ liệu mới
    private void ClearDisplay()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
    }
}