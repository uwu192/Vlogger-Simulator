using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    public bool IsFilled { get; private set; }

    private void Awake()
    {
        // Tự động lấy Image nếu chưa kéo vào Inspector
        if (image == null) image = GetComponent<Image>();
    }

    public void Fill(PickUpFragment fragment)
    {
        if (fragment == null)
        {
            Debug.Log("Không có fragment");
            return;
        }
        if (fragment.Sprite == null)
        {
            Debug.Log($"Mảnh {fragment.gameObject.name} chưa được gán Sprite trong Image component!");
            return;
        }

        if (image != null)
        {
            image.sprite = fragment.Sprite;
            image.color = Color.white; // Đảm bảo hiển thị đúng màu của Sprite
            image.enabled = true;
            IsFilled = true;
            Debug.Log("Đã ghép ảnh thành công");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager.Instance.TryPlace(this);
    }
}