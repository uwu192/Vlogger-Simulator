using System.Collections.Generic;
using UnityEngine;

public class CatalystHolder : MonoBehaviour
{
    public static CatalystHolder Instance { get; private set; }

    [Header("Cấu hình Kho Catalyst")]
    [SerializeField] private Transform contentParent; // Panel có gắn GridLayoutGroup
    [SerializeField] private GameObject pickUpPrefab;

    [Header("Dữ liệu Catalyst (Kéo 2 loại từ Asset)")]
    [SerializeField] private CatalystData soundCatalystData;
    [SerializeField] private CatalystData cameraCatalystData;

    // Lưu trữ cố định 2 UI Slot đại diện cho 2 loại Catalyst
    private PickUpFragment soundSlot;
    private PickUpFragment cameraSlot;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        GenerateFixedSlots();
    }

    // Khởi tạo duy nhất 2 Slot đại diện trên Panel lưới
    private void GenerateFixedSlots()
    {
        Transform parent = contentParent != null ? contentParent : transform;

        // 1. Tạo ô đại diện Sound Catalyst (Số lượng khởi tạo = 3)
        if (soundCatalystData != null && pickUpPrefab != null)
        {
            GameObject objSound = Instantiate(pickUpPrefab, parent);
            soundSlot = objSound.GetComponent<PickUpFragment>();
            if (soundSlot == null) soundSlot = objSound.AddComponent<PickUpFragment>();

            soundSlot.SetupCatalyst(soundCatalystData, 3);
        }

        // 2. Tạo ô đại diện Camera Catalyst (Số lượng khởi tạo = 3)
        if (cameraCatalystData != null && pickUpPrefab != null)
        {
            GameObject objCam = Instantiate(pickUpPrefab, parent);
            cameraSlot = objCam.GetComponent<PickUpFragment>();
            if (cameraSlot == null) cameraSlot = objCam.AddComponent<PickUpFragment>();

            cameraSlot.SetupCatalyst(cameraCatalystData, 3);
        }
    }

    // Gọi hàm này khi ĐẶT 1 Catalyst vào ô Ma trận -> Giảm số lượng đi 1
    public void ConsumeCatalyst(CatalystType type)
    {
        if (type == CatalystType.Sound && soundSlot != null)
        {
            soundSlot.AddCount(-1);
        }
        else if (type == CatalystType.Camera && cameraSlot != null)
        {
            cameraSlot.AddCount(-1);
        }
    }

    // Gọi hàm này khi THÁO 1 Catalyst từ ô Ma trận về kho -> Tăng số lượng lên 1
    public void AddCatalystToHolder(CatalystData data)
    {
        if (data == null) return;

        if (data.catalystType == CatalystType.Sound && soundSlot != null)
        {
            soundSlot.AddCount(1);
        }
        else if (data.catalystType == CatalystType.Camera && cameraSlot != null)
        {
            cameraSlot.AddCount(1);
        }
    }
}