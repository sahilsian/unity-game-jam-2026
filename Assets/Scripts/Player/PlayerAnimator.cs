using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator bodyAnimator;
    public MaskSystem maskSystem;  // reference to MaskSystem instead
    
    private Vector2 lastDirection = Vector2.down;

    public void UpdateAnimation(Vector2 movement)
    {
        bool isMoving = movement != Vector2.zero;
        
        if (isMoving)
        {
            lastDirection = movement;
            bodyAnimator.speed = 0.3f;
        }
        else
        {
            bodyAnimator.speed = 0f;
        }

        bodyAnimator.SetFloat("MoveX", lastDirection.x);
        bodyAnimator.SetFloat("MoveY", lastDirection.y);
        bodyAnimator.SetBool("IsMoving", isMoving);

        // Update mask through MaskSystem
        if (maskSystem != null)
        {
            maskSystem.UpdateMaskAnimation(lastDirection, isMoving);
        }
    }
}
