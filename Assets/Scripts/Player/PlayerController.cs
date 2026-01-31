using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float crouchSpeed = 2.5f;
    
    private float currentSpeed;
    private bool isCrouching = false;
    
    void Start()
    {
        currentSpeed = walkSpeed;
    }
    
    void Update()
    {
        HandleCrouchInput();
        HandleMovement();
    }
    
    void HandleCrouchInput()
    {
        // Toggle crouch with left ctrl key
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            currentSpeed = isCrouching ? crouchSpeed : walkSpeed;
        }
    }
    
    void HandleMovement()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Create movement vector
        Vector2 movement = new Vector2(horizontal, vertical);
        
        // Normalize to prevent faster diagonal movement
        // should implelemnt diagonal later
        movement = movement.normalized;
        
        // Apply movement
        transform.Translate(movement * currentSpeed * Time.deltaTime);
    }
}