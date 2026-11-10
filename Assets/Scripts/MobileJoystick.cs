using UnityEngine;
using UnityEngine.EventSystems;

public class MobileJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform joystickBG;
    public float moveRange = 75f;

    public Vector2 MoveInput { get; private set; }

    private RectTransform knob;

    private void Start()
    {
        knob = GetComponent<RectTransform>();
        // Ép Knob về đúng tâm khi mới vào game
        knob.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Dịch tọa độ màn hình vật lý về tọa độ UI bên trong khung JoystickBG
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickBG, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            // Giới hạn núm không văng ra ngoài khung (dựa theo moveRange)
            if (localPoint.magnitude > moveRange)
            {
                localPoint = localPoint.normalized * moveRange;
            }

            // Cập nhật vị trí núm bằng tọa độ UI (anchoredPosition)
            knob.anchoredPosition = localPoint;

            // Truyền tín hiệu (Vector2) từ -1 đến 1 cho nhân vật chạy
            MoveInput = localPoint / moveRange;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Thả tay ra tự động bật về giữa
        knob.anchoredPosition = Vector2.zero;
        MoveInput = Vector2.zero;
    }
}