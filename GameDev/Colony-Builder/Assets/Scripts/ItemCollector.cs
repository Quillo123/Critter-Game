using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Inventory))]
public class ItemCollector : MonoBehaviour
{
    private Inventory inventory;


    void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    // Collect Item when Collided With
    private void OnCollisionEnter2D(Collision2D collision)
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
