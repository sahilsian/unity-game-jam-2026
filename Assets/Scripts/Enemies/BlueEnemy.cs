using UnityEngine;

public class BlueEnemy : EnemyBase
{
    [Header("Linear Patrol Settings")]
    [SerializeField] private float moveDuration = 2.0f;
    [SerializeField] private float pauseDuration = 1.0f;
    
    private float timer;
    private bool isMoving = true;
    private Rigidbody2D rb;
    
    private int directionToggle = 0; 
    private float startAngle;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        
        startAngle = transform.eulerAngles.z;
        
        timer = moveDuration;
        isMoving = true;
    }

    protected override void PerformBehavior()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (isMoving)
            {
                isMoving = false;
                timer = pauseDuration;
                rb.linearVelocity = Vector2.zero; 
            }
            else
            {
                directionToggle = (directionToggle + 1) % 2;
                
                float targetAngle = startAngle + (directionToggle * 180f);
                transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                
                transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

                isMoving = true;
                timer = moveDuration;
            }
        }

        if (isMoving)
        {
            rb.MovePosition(rb.position + (Vector2)transform.right * moveSpeed * Time.deltaTime);
        }
    }
}