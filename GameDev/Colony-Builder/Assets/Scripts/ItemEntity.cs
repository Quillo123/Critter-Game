using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ItemEntity : MonoBehaviour
{
    ItemDatabase itemDB => ItemDatabase.Instance;


    public Item item;
    private SpriteRenderer spriteRenderer;

    private float pickupTime = 0;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        GetComponent<Collider2D>().isTrigger = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        spriteRenderer.sprite = item.sprite;
    }

    public void SetItem(string itemID)
    {
        item = itemDB.GetItemByID(itemID);
        spriteRenderer.sprite = item.sprite;
    }

    public void SetCooldown(float cooldown)
    {
        pickupTime = Time.time + cooldown;
    }

    public bool CanPickUp()
    {
        return Time.time > pickupTime;
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
