using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    public MaskSystem maskSystem;
    
    [Header("UI Mask Slots")]
    public Image redMaskSlot;
    public Image blueMaskSlot;
    public Image greenMaskSlot;
    public Image yellowMaskSlot;
    
    [Header("Visual Settings")]
    public Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    public Color unlockedColor = Color.white;
    public Color activeColor = Color.yellow;
    
    void Start()
    {
        // Initial HUD setup
        if (maskSystem == null)
        {
            Debug.LogError("MaskSystem reference is missing! Please assign it in the Inspector.");
            return;
        }
        
        UpdateHUD();
    }
    
    void Update()
    {
        // Continuously update the HUD to reflect current mask state
        UpdateHUD();
    }
    
    void UpdateHUD()
    {
        if (maskSystem == null) return;
        
        // Update each mask slot
        UpdateMaskSlot(redMaskSlot, MaskSystem.MaskType.Red);
        UpdateMaskSlot(blueMaskSlot, MaskSystem.MaskType.Blue);
        UpdateMaskSlot(greenMaskSlot, MaskSystem.MaskType.Green);
        UpdateMaskSlot(yellowMaskSlot, MaskSystem.MaskType.Yellow);
    }
    
    void UpdateMaskSlot(Image slotImage, MaskSystem.MaskType maskType)
    {
        if (slotImage == null) return;
        
        // Set the sprite from MaskSystem
        slotImage.sprite = maskSystem.GetMaskSprite(maskType);
        
        // Check if this mask is unlocked
        bool isUnlocked = maskSystem.HasMask(maskType);
        
        // Set the color based on state
        if (!isUnlocked)
        {
            // Locked/not collected yet - grayed out
            slotImage.color = lockedColor;
        }
        else if (maskSystem.currentMask == maskType)
        {
            // Currently active mask - highlighted
            slotImage.color = activeColor;
        }
        else
        {
            // Unlocked but not active - normal color
            slotImage.color = unlockedColor;
        }
    }
}