using UnityEngine;
using UnityEngine.InputSystem;

public class IsometricPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Giao diện Mobile")]
    public MobileJoystick joystick; // Đã thêm: Khai báo cổng cắm Joystick ảo

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 movement;
    private InputAction moveAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        moveAction = new InputAction("Move", expectedControlType: "Vector2");

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");

        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
    }

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
        // Đọc tín hiệu từ bàn phím PC
        movement = moveAction.ReadValue<Vector2>();

        // ĐÃ THÊM: Nếu có thao tác kéo Joystick ảo, lấy dữ liệu Joystick đè lên dữ liệu bàn phím
        if (joystick != null && joystick.MoveInput != Vector2.zero)
        {
            movement = joystick.MoveInput;
        }

        if (movement != Vector2.zero)
        {
            anim.SetFloat("InputX", movement.x);
            anim.SetFloat("InputY", movement.y);
        }

        anim.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}