using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float crouchSpeed = 2.5f;

    [Header("References")]
    public PlayerAnimator playerAnimator;  // assign in Inspector

    private Rigidbody2D rb;
    private float currentSpeed;
    private bool isCrouching = false;
    private Vector2 movement;

    void Start()
    {
        currentSpeed = walkSpeed;
        rb = GetComponent<Rigidbody2D>();

        // Auto-find PlayerAnimator if not assigned
        if (playerAnimator == null)
        {
            playerAnimator = GetComponent<PlayerAnimator>();
        }
    }

    void Update()
    {
        HandleCrouchInput();
        HandleMovementInput();
    }

    void FixedUpdate()
    {
        // Use Rigidbody2D for physics-based movement (better for collisions)
        rb.MovePosition(rb.position + movement * currentSpeed * Time.fixedDeltaTime);
    }

    void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
        }
    }

    void HandleMovementInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.magnitude > 1f)
        {
            movement = movement.normalized;
        }

        // Update animations
        playerAnimator.UpdateAnimation(movement);
    }
}