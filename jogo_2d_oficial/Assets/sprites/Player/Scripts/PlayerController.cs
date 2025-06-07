using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;
    private Vector2 lastMovement = Vector2.down;

    private PlayerInputActions actions;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        actions = new PlayerInputActions();
    }

    void OnEnable()
    {
        actions.Enable();
    }

    void OnDisable()
    {
        actions.Disable();
    }

    void Update()
    {
        movement = actions.Gameplay.Move.ReadValue<Vector2>();

        if (movement != Vector2.zero)
            lastMovement = movement.normalized;

        animator.SetFloat("MoveX", lastMovement.x);
        animator.SetFloat("MoveY", lastMovement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    void FixedUpdate()
    {
        if (movement != Vector2.zero)
        {
            Vector2 desloc = movement.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + desloc);
        }
    }
}