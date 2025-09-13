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

    // Collect Item when Collided With
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collecting Item");
       
        var item = collision.gameObject.GetComponent<ItemEntity>();
        if (item)
        {
            if (inventory.AddItem(item))
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}
