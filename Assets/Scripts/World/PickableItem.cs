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
        if (item == null)
        {
            Debug.Log("could not pick up");
        }
        ;
        Debug.Log("PICKED UP: ", item);
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
