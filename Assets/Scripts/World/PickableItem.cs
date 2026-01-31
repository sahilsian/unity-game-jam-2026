using UnityEngine;
using UnityEngine.Events;

public class PickableItem : MonoBehaviour
{
    [Header("Item")]
    [SerializeField] private Item item;
    [SerializeField] private int amount = 1;

    [Header("Pickup")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    [SerializeField] private bool destroyOnPickup = true;

    public UnityEvent<Item, int> onPickedUp;

    private bool playerInRange;

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(pickupKey))
            Pickup();
    }

    private void Pickup()
    {
        if (item == null) return;

        // Inventory hook goes here
        // Inventory.Instance.Add(item, amount);

        onPickedUp?.Invoke(item, amount);

        if (destroyOnPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
            playerInRange = false;
    }
}
