// Store what masks the player has collected
// store what mask is currently equipped
// handle equipping and unequipping masks
// What does each mask do? 

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
    
    [Header("Mask Sprites")]
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

    void Start()
    {
        // Try to load sprites from Assets/Art/Items
        if (redMaskSprite == null)
            redMaskSprite = Resources.Load<Sprite>("Art/Items/red-mask");
        if (blueMaskSprite == null)
            blueMaskSprite = Resources.Load<Sprite>("Art/Items/blue-mask");
        if (greenMaskSprite == null)
            greenMaskSprite = Resources.Load<Sprite>("Art/Items/green-mask");
        if (yellowMaskSprite == null)
            yellowMaskSprite = Resources.Load<Sprite>("Art/Items/yellow-mask");
            
        Debug.Log("Red mask loaded: " + (redMaskSprite != null));
    }
    
    void Update()
    {
        HandleMaskSwitching();
    }
    
    void HandleMaskSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hasRedMask)
            SwitchMask(MaskType.Red);
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && hasBlueMask)
            SwitchMask(MaskType.Blue);
            
        if (Input.GetKeyDown(KeyCode.Alpha3) && hasGreenMask)
            SwitchMask(MaskType.Green);
            
        if (Input.GetKeyDown(KeyCode.Alpha4) && hasYellowMask)
            SwitchMask(MaskType.Yellow);
    }
    
    void SwitchMask(MaskType newMask)
    {
        currentMask = newMask;
        Debug.Log("Switched to: " + newMask + " mask");
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