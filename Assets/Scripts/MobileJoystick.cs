using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBG;
    public float moveRange = 75f;
    
    public Vector2 MoveInput { get; private set; }

    private RectTransform knob;
    private Vector2 touchStartLocal; // Biến mới: Nhớ vị trí ngón tay lúc vừa chạm

    private void Start()
    {
        knob = GetComponent<RectTransform>();
        knob.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData) 
    { 
        // 1. Lấy vị trí ngón tay ngay khoảnh khắc chạm vào màn hình làm MỐC
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG, eventData.position, eventData.pressEventCamera, out touchStartLocal);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG, eventData.position, eventData.pressEventCamera, out Vector2 currentTouchLocal))
        {
            // 2. Chỉ tính sự chênh lệch (vuốt đi bao xa) so với mốc ban đầu
            Vector2 direction = currentTouchLocal - touchStartLocal;

            if (direction.magnitude > moveRange)
            {
                direction = direction.normalized * moveRange;
            }

            knob.anchoredPosition = direction;
            MoveInput = direction / moveRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        knob.anchoredPosition = Vector2.zero;
        MoveInput = Vector2.zero;
    }
}