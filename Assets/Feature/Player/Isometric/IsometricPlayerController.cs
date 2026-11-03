using UnityEngine;
using UnityEngine.InputSystem; // Bắt buộc phải khai báo thư viện mới

public class IsometricPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;

    // Khai báo biến quản lý thao tác di chuyển thay thế cho Input cũ
    private InputAction moveAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // Khởi tạo hành động di chuyển dạng Vector2 (tương tự vector của Joystick)
        moveAction = new InputAction("Move", expectedControlType: "Vector2");

        // Gán cụm phím WASD vào action
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        // Cấu hình thêm cụm phím mũi tên để có nhiều lựa chọn điều khiển
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
    }

    // Input System mới bắt buộc phải kích hoạt Action thủ công
    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();

        // Chỉ cập nhật hướng nhìn (InputX, InputY) khi người chơi có thao tác bấm phím.
        // Khi nhả phím, movement = 0, đoạn if này bị bỏ qua giúp nhân vật NHỚ hướng mặt cuối cùng.
        if (movement != Vector2.zero)
        {
            anim.SetFloat("InputX", movement.x);
            anim.SetFloat("InputY", movement.y);
        }

        // Truyền giá trị độ lớn của vector vào biến Speed trong Animator
        // sqrMagnitude sẽ trả về số > 0 khi có di chuyển, và bằng 0 khi nhả phím
        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}