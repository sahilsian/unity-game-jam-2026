using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float moveSpeed = 3.0f;
    public static bool IsPlayerSpotted = false;
    
    protected Transform playerTransform;
    protected GameObject VisionSprite;

    // Timer variables
    private float pursuitTimer = 2.0f; // 2 second wait
    private bool hasStartedPursuit = false;

    protected virtual void Start()
    {
        Transform coneTransform = transform.Find("VisionSprite");
        if (coneTransform != null) VisionSprite = coneTransform.gameObject;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    protected virtual void Update()
    {
        if (IsPlayerSpotted)
        {
            // 1. Hide the vision cone immediately
            if (VisionSprite != null && VisionSprite.activeSelf) 
            {
                VisionSprite.SetActive(false);
            }

            // 2. Handle the 2-second wait
            if (pursuitTimer > 0)
            {
                pursuitTimer -= Time.deltaTime; // Count down
                // Optional: You could make the enemy shake or turn red here to show they are "alerted"
            }
            else
            {
                PursuePlayer(); // Only run this once the timer hits 0
            }
        }
        else
        {
            PerformBehavior();
        }
    }

    protected virtual void PerformBehavior() { }

    protected virtual void PursuePlayer()
    {
        if (playerTransform == null) return;

        // Move towards the player
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}