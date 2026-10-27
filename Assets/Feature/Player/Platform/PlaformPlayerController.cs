using UnityEngine;
using UnityEngine.InputSystem;

public class MovePlatformPlayer : MonoBehaviour
{
    // Đổi tên biến từ rigidbody thành rb để tránh trùng với từ khóa hệ thống của Unity
    private Rigidbody2D rb;
    [SerializeField] float speed = 10.0f;
    private float direction;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        direction = 0f;
        if (Keyboard.current.aKey.isPressed) direction = -1f;
        else if (Keyboard.current.dKey.isPressed) direction = 1f;
    }

    void FixedUpdate()
    {
        // 1. Sửa sang Vector2 phù hợp với Rigidbody2D
        // 2. Thay Time.deltaTime thành Time.fixedDeltaTime
        Vector2 targetPosition = rb.position + new Vector2(direction, 0f) * speed * Time.fixedDeltaTime;

        rb.MovePosition(targetPosition);
    }
}