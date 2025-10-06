using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemCollector : MonoBehaviour
{
    public Inventory inventory;


    void Start()
    {
        if (!inventory)
        {
            inventory = GetComponent<Inventory>();
            if (!inventory)
            {
                inventory = GetComponentInParent<Inventory>();
            }
        }
    }

    float CheckForItem = 0;
    // Collect Item when Collided With
    private void OnTriggerStay2D(Collider2D collision)
    {
        var item = collision.gameObject.GetComponent<ItemEntity>();
        if (item && item.CanPickUp())
        {
            if (inventory.AddItem(item.item.itemID, 1) == 1)
            {
                Destroy(collision.gameObject);
            }
            else
            {
                Logger.LogError("Something went wrong inserting the ITEM!", gameObject);
            }
        }
    }
}
