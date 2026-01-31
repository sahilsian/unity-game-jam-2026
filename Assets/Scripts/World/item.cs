using UnityEngine;
using UnityEngine.Events;

public class PickableItem : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private KeyCode pickupKey = KeyCode.E;
    [SerializeField] private bool destroyOnPickup = true;

    [Header("Optional UI Prompt")]
    [SerializeField] private GameObject pickupPrompt;

    [Header("Item Data")]
    [SerializeField] private string itemId = "item_name";
    [SerializeField] private int amount = 1;

    [Header("Events")]
    public UnityEvent onPickedUp;

    private bool playerInRange;

    private void Awake()
    {
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(pickupKey))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        // TODO: Hook into your inventory system here
        // Example:
        // Inventory.Instance.Add(itemId, amount);

        onPickedUp?.Invoke();

        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);

        if (destroyOnPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = true;
        if (pickupPrompt != null)
            pickupPrompt.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = false;
        if (pickupPrompt != null)
            pickupPrompt.SetActive(false);
    }
}
