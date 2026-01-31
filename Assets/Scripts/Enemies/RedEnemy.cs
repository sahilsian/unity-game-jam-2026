using UnityEngine;

public class RedEnemy : EnemyBase
{
    [Header("Whole Body Rotation")]
    [SerializeField] private float rotationSpeed = 1.5f;
    [SerializeField] private float sweepAngle = 30.0f;

    private float startAngle;

    protected override void Start()
    {

        base.Start(); 
        startAngle = transform.eulerAngles.z;

    }

    protected override void PerformBehavior()
    {

        float zRotation = Mathf.Sin(Time.time * rotationSpeed) * sweepAngle;
        transform.rotation = Quaternion.Euler(0, 0, startAngle + zRotation);

    }
}