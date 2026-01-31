using UnityEngine;

public class MaskSystem : MonoBehaviour
{
    public enum MaskType
    {
        None,
        Red,
        Blue,
        Green,
        Yellow
    }
    
    [Header("References")]
    public Animator maskAnimator;
    public SpriteRenderer maskRenderer;
    
    [Header("Mask Animator Controllers")]
    public RuntimeAnimatorController redMaskController;
    public RuntimeAnimatorController blueMaskController;
    public RuntimeAnimatorController greenMaskController;
    public RuntimeAnimatorController yellowMaskController;
    
    [Header("Mask Sprites (for UI/inventory)")]
    public Sprite redMaskSprite;
    public Sprite blueMaskSprite;
    public Sprite greenMaskSprite;
    public Sprite yellowMaskSprite;
    
    [Header("Current State")]
    public MaskType currentMask = MaskType.None;
    
    [Header("Unlocked Masks")]
    public bool hasRedMask = false;
    public bool hasBlueMask = false;
    public bool hasGreenMask = false;
    public bool hasYellowMask = false;

    private Vector2 lastDirection = Vector2.down;
    private bool isMoving = false;

    void Start()
    {
        // Hide mask at start if none equipped
        if (maskRenderer != null && currentMask == MaskType.None)
        {
            maskRenderer.enabled = false;
        }
    }
    
    void Update()
    {
        HandleMaskSwitching();
    }
    
    void HandleMaskSwitching()
    {
        // Press same key to toggle off
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (hasRedMask)
                ToggleMask(MaskType.Red);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hasBlueMask)
                ToggleMask(MaskType.Blue);
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (hasGreenMask)
                ToggleMask(MaskType.Green);
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (hasYellowMask)
                ToggleMask(MaskType.Yellow);
        }
        
        // Press 0 or Q to remove mask
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Q))
        {
            RemoveMask();
        }
    }
    
    void ToggleMask(MaskType maskType)
    {
        if (currentMask == maskType)
        {
            RemoveMask();
        }
        else
        {
            EquipMask(maskType);
        }
    }
    
public void EquipMask(MaskType maskType)
{
    currentMask = maskType;
    
    RuntimeAnimatorController controller = GetMaskController(maskType);
    
    if (controller != null && maskAnimator != null)
    {
        maskRenderer.enabled = true;
        maskAnimator.runtimeAnimatorController = controller;
        
        // Force down direction immediately
        maskAnimator.SetFloat("MoveX", 0f);
        maskAnimator.SetFloat("MoveY", -1f);
        maskAnimator.SetBool("IsMoving", false);
        
        // Force animator to update
        maskAnimator.Update(0f);
        
        // Then apply actual current state
        UpdateMaskAnimation(lastDirection, isMoving);
    }
    
    Debug.Log("Equipped: " + maskType + " mask");
}
    
    public void RemoveMask()
    {
        currentMask = MaskType.None;
        
        if (maskRenderer != null)
        {
            maskRenderer.enabled = false;
        }
        
        Debug.Log("Removed mask");
    }
    
    // Called by PlayerAnimator to sync mask with body
    public void UpdateMaskAnimation(Vector2 direction, bool moving)
    {
        lastDirection = direction;
        isMoving = moving;
        
        if (maskAnimator == null || currentMask == MaskType.None)
            return;
        
        // Hide mask when facing up (back of head)
        bool facingUp = direction.y > 0.2f && Mathf.Abs(direction.x) < 0.2f;
        maskRenderer.enabled = !facingUp;
        
        if (facingUp)
            return;
        
        // Sync animation with body
        maskAnimator.SetFloat("MoveX", direction.x);
        maskAnimator.SetFloat("MoveY", direction.y);
        maskAnimator.SetBool("IsMoving", moving);
        
        // Match body animator speed
        maskAnimator.speed = moving ? 1f : 0f;
    }
    
    public void CollectMask(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Red:
                hasRedMask = true;
                break;
            case MaskType.Blue:
                hasBlueMask = true;
                break;
            case MaskType.Green:
                hasGreenMask = true;
                break;
            case MaskType.Yellow:
                hasYellowMask = true;
                break;
        }
        
        Debug.Log("Collected: " + maskType + " mask");
    }
    
    public RuntimeAnimatorController GetMaskController(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Red: return redMaskController;
            case MaskType.Blue: return blueMaskController;
            case MaskType.Green: return greenMaskController;
            case MaskType.Yellow: return yellowMaskController;
            default: return null;
        }
    }
    
    public Sprite GetMaskSprite(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Red: return redMaskSprite;
            case MaskType.Blue: return blueMaskSprite;
            case MaskType.Green: return greenMaskSprite;
            case MaskType.Yellow: return yellowMaskSprite;
            default: return null;
        }
    }
    
    public bool HasMask(MaskType maskType)
    {
        switch (maskType)
        {
            case MaskType.Red: return hasRedMask;
            case MaskType.Blue: return hasBlueMask;
            case MaskType.Green: return hasGreenMask;
            case MaskType.Yellow: return hasYellowMask;
            default: return false;
        }
    }
}