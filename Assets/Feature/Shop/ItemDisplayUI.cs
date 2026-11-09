using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplayUI : MonoBehaviour
{
    [Header("Dữ liệu & Cấu hình")]
    public ItemCatalog catalog;            // Kho chứa dữ liệu tổng
    public GameObject itemFramePrefab;     // Prefab của ItemFrame (khung chứa UI)
    public Transform contentTransform;     // Vị trí Content trong Scroll View để sinh ra Grid
    public Button cameraTab;
    public Button decorTab;
    private void Start()
    {
        // Hiển thị danh sách Camera ngay khi bật Shop lên (dành cho test)
        DisplayCameras();
    }

    // Hàm gọi để hiển thị riêng danh sách Camera

    private void Update()
    {
        if (cameraTab.IsPressed()) DisplayCameras();
        else if (decorTab.IsPressed()) DisplayDecor();
    }
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

    public void DisplayDecor()
    {
        ClearDisplay();
        List<ItemData> decorItems = catalog.allItems.FindAll(item => item.GetType() == typeof(ItemData));
        foreach (ItemData decorItem in decorItems)
        {
            GameObject newFrame = Instantiate(itemFramePrefab, contentTransform);

            ItemSlotUI slotUI = newFrame.GetComponent<ItemSlotUI>();
            if(slotUI != null)
            {
                slotUI.SetupSlot(decorItem);
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