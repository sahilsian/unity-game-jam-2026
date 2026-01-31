using UnityEngine;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MaskSystem maskSystem;

    [Header("UI Mask Slots")]
    [SerializeField] private Image redMaskSlot;
    [SerializeField] private Image blueMaskSlot;
    [SerializeField] private Image greenMaskSlot;
    [SerializeField] private Image goldMaskSlot;

    [Header("Visual Settings")]
    [SerializeField] private Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 0.5f);
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Color activeColor = Color.yellow;

    private void OnEnable()
    {
        if (maskSystem != null)
            maskSystem.OnChanged += UpdateHUD;
    }

    private void OnDisable()
    {
        if (maskSystem != null)
            maskSystem.OnChanged -= UpdateHUD;
    }

    private void Start()
    {
        if (maskSystem == null)
        {
            Debug.LogError("HUDController is missing MaskSystem reference.", this);
            return;
        }

        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (maskSystem == null) return;

        UpdateSlot(redMaskSlot, "redMask");
        UpdateSlot(blueMaskSlot, "blueMask");
        UpdateSlot(greenMaskSlot, "greenMask");
        UpdateSlot(goldMaskSlot, "goldMask");
    }

    private void UpdateSlot(Image slot, string id)
    {
        if (slot == null) return;

        Item item = maskSystem.GetMaskItem(id);
        slot.sprite = item != null ? item.icon : null;

        bool unlocked = maskSystem.HasMask(id);

        if (!unlocked) slot.color = lockedColor;
        else if (maskSystem.CurrentMask != null && maskSystem.CurrentMask.id == id) slot.color = activeColor;
        else slot.color = unlockedColor;
    }
}
