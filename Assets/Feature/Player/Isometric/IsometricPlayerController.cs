using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class IsometricPlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4f;

    private static readonly Vector2 IsoRight = new(1f, 0.5f);
    private static readonly Vector2 IsoUp = new(-1f, 0.5f);  

    private Rigidbody2D rb;
    private Vector2 isoDirection;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) { isoDirection = Vector2.zero; return; }

        Vector2 input = Vector2.zero;
        if (kb.aKey.isPressed) input.x -= 1f;
        if (kb.dKey.isPressed) input.x += 1f;
        if (kb.sKey.isPressed) input.y -= 1f;
        if (kb.wKey.isPressed) input.y += 1f;

        isoDirection = (IsoRight * input.x + IsoUp * input.y).normalized;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + isoDirection * speed * Time.fixedDeltaTime);
    }
}