using UnityEngine;

public class BlueEnemy : EnemyBase
{
    [Header("Square Patrol Settings")]
    [SerializeField] private float moveDuration = 2.0f;
    [SerializeField] private float pauseDuration = 1.0f;
    
    private float timer;
    private bool isMoving = false;
    private Rigidbody2D rb;
    
    // Track which side of the square we are on (0, 1, 2, 3)
    private int currentDirectionIndex = 0; 

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        timer = pauseDuration;
    }

    protected override void PerformBehavior()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (!isMoving)
            {
                // 1. SNAP to a perfect angle based on the side of the square
                // This prevents "drift" over time
                currentDirectionIndex = (currentDirectionIndex + 1) % 4;
                float targetAngle = currentDirectionIndex * 90f;
                
                transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                
                isMoving = true;
                timer = moveDuration;
            }
            else
            {
                isMoving = false;
                timer = pauseDuration;
            }
        }

        if (isMoving)
        {
            // Move using MovePosition so physics keeps it from overlapping walls
            rb.MovePosition(rb.position + (Vector2)transform.right * moveSpeed * Time.deltaTime);
        }
    }
}