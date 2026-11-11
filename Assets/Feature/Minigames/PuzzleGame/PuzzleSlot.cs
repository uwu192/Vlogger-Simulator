using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleSlot : MonoBehaviour, IPointerClickHandler
{
    [Header("UI References")]
    [SerializeField] private Image slotImage; // Hình ảnh hiển thị trên ô lưới

    // Các thuộc tính dữ liệu cần thiết cho PuzzleScorer
    public bool IsFilled { get; private set; }
    public bool IsPlacedCatalyst => PlacedCatalystData != null;
    public FragmentData PlacedFragmentData { get; private set; }
    public CatalystData PlacedCatalystData { get; private set; }

    public int GridX { get; private set; }
    public int GridY { get; private set; }

    private void Awake()
    {
        if (slotImage == null)
        {
            slotImage = GetComponent<Image>();
        }
    }

    // PuzzleManager gọi hàm này để gán tọa độ khi khởi tạo lưới
    public void InitCoords(int x, int y)
    {
        GridX = x;
        GridY = y;
    }

    // Hàm đặt mảnh vào ô
    public void Fill(PickUpFragment fragment)
    {
        IsFilled = true;

        // Phân loại dữ liệu được đặt vào
        if (fragment.IsCatalyst)
        {
            PlacedCatalystData = fragment.catalystData;
            PlacedFragmentData = null;
        }
        else
        {
            PlacedFragmentData = fragment.fragmentData;
            PlacedCatalystData = null;
        }

        // Cập nhật UI hiển thị
        if (slotImage != null && fragment.Sprite != null)
        {
            slotImage.sprite = fragment.Sprite;
            slotImage.color = Color.white; // Đảm bảo hiển thị rõ hình ảnh
        }
    }

    // Hàm gỡ mảnh khỏi ô
    public void Clear()
    {
        IsFilled = false;
        PlacedFragmentData = null;
        PlacedCatalystData = null;

        // Xóa hiển thị UI
        if (slotImage != null)
        {
            slotImage.sprite = null;
            slotImage.color = new Color(1, 1, 1, 0); // Làm trong suốt khi không có mảnh
        }
    }

    // Bắt sự kiện người dùng click vào ô này
    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager.Instance.OnSlotClicked(this);
    }
}