using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PickUpFragment : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image image;

    public Sprite Sprite => image != null ? image.sprite : null;

    public void OnPointerClick(PointerEventData eventData)
    {
        PuzzleManager.Instance.Select(this);
        Debug.Log($"Đã chọn mảnh: {gameObject.name}");
    }
}