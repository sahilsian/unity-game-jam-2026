using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator bodyAnimator;
    public Animator maskAnimator;  // optional, can be null

    private Vector2 lastDirection = Vector2.down;

    public void UpdateAnimation(Vector2 movement)
    {
        bool isMoving = movement != Vector2.zero;

        if (isMoving)
        {
            lastDirection = movement;
            bodyAnimator.speed = 1.0f;
        } else
        {
            bodyAnimator.speed = 0.0f;
        }

        // Remember last direction for idle facing
        if (movement != Vector2.zero)
        {
            lastDirection = movement;
        }

        // Update body animator
        bodyAnimator.SetFloat("MoveX", lastDirection.x);
        bodyAnimator.SetFloat("MoveY", lastDirection.y);
        bodyAnimator.SetBool("IsMoving", movement != Vector2.zero);

        // Update mask animator if assigned
        if (maskAnimator != null)
        {
            maskAnimator.SetFloat("MoveX", lastDirection.x);
            maskAnimator.SetFloat("MoveY", lastDirection.y);
            maskAnimator.SetBool("IsMoving", movement != Vector2.zero);
        }
    }
}