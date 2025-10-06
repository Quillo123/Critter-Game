using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Hotbar : MonoBehaviour
{
    public static int size = 10;

    ItemDatabase itemdb => ItemDatabase.Instance;

    public Inventory inventory;
    public ItemHolder holder;

    Item[] items;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (inventory == null)
        {
            inventory = GetComponent<Inventory>();
        }

        if (inventory != null)
        {
            inventory.OnInventoryChanged += UpdateHotbar;
        }
        else Logger.LogError("Missing inventory", gameObject);

        if (holder == null)
        {
            Logger.LogError("Missing item holder", gameObject);
        }

        items = new Item[size];
    }

    public void AddItem(string itemID, int index)
    {
        if(index < 0 || index >= size)
        {
            Logger.Log("Index out of range", gameObject);
        }

        Item item = itemdb.GetItemByName(itemID);
        if(item != null)
        {
            items[index] = item;
        }
        else
        {
            Logger.LogError($"item '{itemID}' does not exist", gameObject);
        }
    }

    public void UpdateHotbar()
    {
        
    }
}
