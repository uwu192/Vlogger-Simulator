using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickUpFragment : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    [Header("Dữ liệu mảnh (Chỉ kéo 1 trong 2 vào Inspector)")]
    public FragmentData fragmentData;
    public CatalystData catalystData;

    public bool IsCatalyst => catalystData != null;
    public Sprite Sprite => image != null ? image.sprite : null;

    private void Awake()
    {
        // Lấy Component Image nếu chưa được kéo vào Inspector
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        // Gán Sprite lấy theo Rarity từ fragmentData vào component Image
        if (image != null && fragmentData != null)
        {
            image.sprite = fragmentData.GetImage();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager.Instance.Select(this);
        Debug.Log($"Đã chọn mảnh: {gameObject.name}");
    }
}