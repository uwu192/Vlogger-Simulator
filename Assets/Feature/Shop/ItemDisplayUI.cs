using System.Collections.Generic;
using UnityEngine;

public class ItemDisplayUI : MonoBehaviour
{
    [Header("Dữ liệu & Cấu hình")]
    public ItemCatalog catalog;            // Kho chứa dữ liệu tổng
    public GameObject itemFramePrefab;     // Prefab của ItemFrame (khung chứa UI)
    public Transform contentTransform;     // Vị trí Content trong Scroll View để sinh ra Grid

    private void Start()
    {
        // Hiển thị danh sách Camera ngay khi bật Shop lên (dành cho test)
        DisplayCameras();
    }

    // Hàm gọi để hiển thị riêng danh sách Camera
    public void DisplayCameras()
    {
        ClearDisplay(); // Xóa sạch các item cũ trên Grid trước khi tải mới

        // Lấy danh sách chỉ toàn Camera từ Catalog
        List<CameraData> cameraItems = catalog.GetItemsOfType<CameraData>();

        // Duyệt qua từng Camera và tạo UI Frame tương ứng
        foreach (CameraData camera in cameraItems)
        {
            // 1. Sinh ra 1 cái ItemFrame mới, đặt nó làm con của Content
            GameObject newFrame = Instantiate(itemFramePrefab, contentTransform);

            // 2. Tìm script ItemSlotUI trên Frame vừa tạo
            ItemSlotUI slotUI = newFrame.GetComponent<ItemSlotUI>();
            if (slotUI != null)
            {
                // 3. Đổ dữ liệu Camera (hình, giá) vào Frame
                slotUI.SetupSlot(camera);
            }
        }
    }

    // Hàm dọn dẹp các vật phẩm đang hiển thị trong Content
    private void ClearDisplay()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }
    }
}