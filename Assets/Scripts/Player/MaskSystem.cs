using System;
using System.Collections.Generic;
using UnityEngine;

public class MaskSystem : MonoBehaviour
{
    [Header("Mask Items (assign the 4 Item assets)")]
    [SerializeField] private Item redMask;
    [SerializeField] private Item blueMask;
    [SerializeField] private Item greenMask;
    [SerializeField] private Item goldMask;

    // Store owned masks by id
    private readonly HashSet<string> ownedMaskIds = new HashSet<string>();

    public Item CurrentMask { get; private set; }

    // HUD listens to this
    public event Action OnChanged;

    // ---------- Public API ----------

    public bool IsMaskItem(Item item)
    {
        if (item == null) return false;
        return item.id == "redMask" || item.id == "blueMask" || item.id == "greenMask" || item.id == "goldMask";
    }

    public bool HasMask(string id) => ownedMaskIds.Contains(id);

    public Item GetMaskItem(string id)
    {
        return id switch
        {
            "redMask" => redMask,
            "blueMask" => blueMask,
            "greenMask" => greenMask,
            "goldMask" => goldMask,
            _ => null
        };
    }

    // Call this from pickup
    public void Add(Item item)
    {
        if (item == null) return;

        if (!IsMaskItem(item))
            return;

        bool wasNew = ownedMaskIds.Add(item.id);

        // Optional: auto-equip first time you pick it up
        if (wasNew && CurrentMask == null)
            CurrentMask = item;

        OnChanged?.Invoke();
    }

    public bool TryEquip(string id)
    {
        if (!HasMask(id)) return false;

        var item = GetMaskItem(id);
        if (item == null) return false;

        CurrentMask = item;
        OnChanged?.Invoke();
        return true;
    }

    // Convenience: equip by passing the Item directly
    public bool TryEquip(Item item)
    {
        if (item == null) return false;
        return TryEquip(item.id);
    }
}
