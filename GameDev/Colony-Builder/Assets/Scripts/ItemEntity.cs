using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemEntity : MonoBehaviour
{
    ItemDatabase itemDB => ItemDatabase.Instance;


    public Item item;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().isTrigger = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    public void SetItem(string itemID)
    {
        item = itemDB.GetItemByID(itemID);
        spriteRenderer.sprite = item.sprite;
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        Inventory inventory = other.GetComponent<Inventory>();
    //        if (inventory != null && inventory.AddItem(item))
    //        {
    //            Destroy(gameObject);
    //        }
    //    }
    //}
}
