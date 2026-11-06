using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleSlot : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    public int GridX { get; private set; }
    public int GridY { get; private set; }
    public bool IsFilled { get; private set; }

    // Dữ liệu mảnh đang chứa trong Ô
    public FragmentData PlacedFragmentData { get; private set; }
    public bool IsPlacedCatalyst { get; private set; }

    private void Awake()
    {
        if (image == null) image = GetComponent<Image>();
    }

    public void InitCoords(int x, int y)
    {
        GridX = x;
        GridY = y;
    }

    public void Fill(PickUpFragment fragment)
    {
        if (fragment == null || fragment.Sprite == null) return;

        // Gán dữ liệu mảnh
        PlacedFragmentData = fragment.fragmentData;
        IsPlacedCatalyst = fragment.IsCatalyst;
        Debug.Log($"Đặt mảnh vào ô ({GridX},{GridY}) | Data: {(PlacedFragmentData != null ? PlacedFragmentData.fragmentName : "NULL!")} | Catalyst: {IsPlacedCatalyst}");
        image.sprite = fragment.Sprite;
        image.color = Color.white;
        image.enabled = true;
        IsFilled = true;
    }

    public void Clear()
    {
        PlacedFragmentData = null;
        IsPlacedCatalyst = false;
        IsFilled = false;
        
        if (image != null)
        {
            image.sprite = null;
            image.enabled = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager.Instance.OnSlotClicked(this);
    }
}