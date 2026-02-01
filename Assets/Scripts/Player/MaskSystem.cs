using System;
using System.Collections.Generic;
using UnityEngine;

public class MaskSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator maskAnimator;
    [SerializeField] private SpriteRenderer maskRenderer;

    [Header("Mask Items (assign the 4 Item assets)")]
    [SerializeField] private Item redMask;
    [SerializeField] private Item blueMask;
    [SerializeField] private Item greenMask;
    [SerializeField] private Item goldMask;

    [Header("Mask Animator Controllers")]
    [SerializeField] private RuntimeAnimatorController redMaskController;
    [SerializeField] private RuntimeAnimatorController blueMaskController;
    [SerializeField] private RuntimeAnimatorController greenMaskController;
    [SerializeField] private RuntimeAnimatorController goldMaskController;

    // Store owned masks by id
    private readonly HashSet<string> ownedMaskIds = new HashSet<string>();

    // Current equipped mask item (null = none)
    public Item CurrentMask { get; private set; }

    // HUD can listen to this
    public event Action OnChanged;

    private Vector2 lastDirection = Vector2.down;
    private bool isMoving = false;

    // ---------- Unity ----------

    private void Start()
    {
        // Hide mask at start if none equipped
        if (maskRenderer != null && CurrentMask == null)
            maskRenderer.enabled = false;
    }

    private void Update()
    {
        HandleMaskSwitching();
    }

    // ---------- Input ----------

    private void HandleMaskSwitching()
    {
        // Press same key to toggle off
        if (Input.GetKeyDown(KeyCode.Alpha1)) ToggleMask("redMask");
        if (Input.GetKeyDown(KeyCode.Alpha2)) ToggleMask("blueMask");
        if (Input.GetKeyDown(KeyCode.Alpha3)) ToggleMask("greenMask");
        if (Input.GetKeyDown(KeyCode.Alpha4)) ToggleMask("goldMask");

        // Press 0 or Q to remove mask
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Q))
            RemoveMask();
    }

    private void ToggleMask(string id)
    {
        if (!HasMask(id)) return;

        // If currently equipped, remove. Otherwise equip.
        if (CurrentMask != null && CurrentMask.id == id)
            RemoveMask();
        else
            TryEquip(id);
    }

    // ---------- Inventory / Public API ----------

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
        if (!IsMaskItem(item)) return;

        bool wasNew = ownedMaskIds.Add(item.id);

        // Optional: auto-equip first time you pick it up
        if (wasNew && CurrentMask == null)
            TryEquip(item.id);
        else
            OnChanged?.Invoke();
    }

    public bool TryEquip(string id)
    {
        if (!HasMask(id)) return false;

        var item = GetMaskItem(id);
        if (item == null) return false;

        CurrentMask = item;
        ApplyMaskVisualsFor(CurrentMask);

        OnChanged?.Invoke();
        return true;
    }

    // Convenience: equip by passing the Item directly
    public bool TryEquip(Item item)
    {
        if (item == null) return false;
        return TryEquip(item.id);
    }

    public void RemoveMask()
    {
        CurrentMask = null;

        if (maskRenderer != null)
            maskRenderer.enabled = false;

        OnChanged?.Invoke();
        Debug.Log("Removed mask");
    }

    // ---------- Animation syncing (call from your PlayerAnimator) ----------

    // Called by PlayerAnimator to sync mask with body
    public void UpdateMaskAnimation(Vector2 direction, bool moving)
    {
        lastDirection = direction;
        isMoving = moving;

        if (maskAnimator == null || maskRenderer == null || CurrentMask == null)
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

    // ---------- Internals ----------

    private void ApplyMaskVisualsFor(Item item)
    {
        if (maskAnimator == null || maskRenderer == null) return;
        if (item == null) return;

        var controller = GetMaskController(item.id);
        if (controller == null) return;

        maskRenderer.enabled = true;
        maskAnimator.runtimeAnimatorController = controller;

        // Force down direction immediately (prevents weird initial frame)
        maskAnimator.SetFloat("MoveX", 0f);
        maskAnimator.SetFloat("MoveY", -1f);
        maskAnimator.SetBool("IsMoving", false);

        // Force animator to update
        maskAnimator.Update(0f);

        // Then apply actual current state
        UpdateMaskAnimation(lastDirection, isMoving);

        Debug.Log("Equipped: " + item.id);
    }

    private RuntimeAnimatorController GetMaskController(string id)
    {
        return id switch
        {
            "redMask" => redMaskController,
            "blueMask" => blueMaskController,
            "greenMask" => greenMaskController,
            "goldMask" => goldMaskController,
            _ => null
        };
    }
}
