using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemID; // Unique identifier for the item
    public string itemName;
    public Sprite sprite;
    public int maxStackSize = 99;



    public Item(string itemID, string itemName, Sprite sprite, int maxStackSize)
    {
        this.itemID = itemID;
        this.itemName = itemName;
        this.sprite = sprite;
        this.maxStackSize = maxStackSize;
    }
}