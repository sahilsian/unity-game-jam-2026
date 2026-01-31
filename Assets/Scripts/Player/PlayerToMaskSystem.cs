using UnityEngine;

public class PickupToMaskSystem : MonoBehaviour
{
    [SerializeField] private MaskSystem maskSystem;

    public void OnPickedUp(Item item, int amount)
    {
        if (maskSystem == null) return;
        maskSystem.Add(item);
    }
}
