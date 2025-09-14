using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Progress;

[Serializable]
public struct StoredItem
{
    public string itemID;
    public int count;

    public StoredItem(Item item, int count)
    {
        this.itemID = item.itemID;
        this.count = count;
    }

    public StoredItem(string itemID, int count)
    {
        this.itemID = itemID;
        this.count = count;
    }
}

public class Inventory : MonoBehaviour
{
    ItemDatabase itemDB => ItemDatabase.Instance;
    public StoredItem[] items;
    public int capacity = 20;

    public event Action OnInventoryChanged;

    private void Start()
    {
        items = new StoredItem[capacity];
    }

    /// <summary>
    /// Returns the number of items inserted
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public int AddItem(string itemID, int count, int index)
    {
        if (index < 0 || index >= capacity)
        {
            return 0; 
        }

        var max = itemDB.GetItemByID(itemID).maxStackSize;
        int itemsLeft = count;

        if (IsSlotEmpty(index))
        {
            items[index].itemID = itemID;
            items[index].count = 0;
        }

        if (items[index].itemID == itemID)
        {

            if (items[index].count + itemsLeft > max)
            {
                items[index].count = max;
                itemsLeft -= max;
            }
            else
            {
                items[index].count += itemsLeft;
                return count;
            }

        }
        OnInventoryChanged?.Invoke();
        return count - itemsLeft;
    }

    /// <summary>
    /// Returns the number of items inserted
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public int AddItem(string itemID, int count)
    {
        var max = itemDB.GetItemByID(itemID).maxStackSize;
        int itemsLeft = count;

        int slot = FindItemSlotWithSpaceOrEmpty(itemID);
        
        while (slot >= 0)
        {
            if (IsSlotEmpty(slot))
            {
                items[slot].itemID = itemID;
                items[slot].count = 0;
            }

            if (items[slot].count + itemsLeft > max)
            {
                items[slot].count = max;
                itemsLeft -= max;
                slot = FindItemSlotWithSpaceOrEmpty(itemID);
            }
            else
            {
                items[slot].count += itemsLeft;
                return count;
            }

        }
        OnInventoryChanged?.Invoke();
        return count - itemsLeft;
    }


    public string RemoveItem(int index)
    {
        if (index >= capacity || index < 0)
        {
            Debug.LogError($"ERROR: Invalid attempt to access inventory (capacity {capacity}) at slot ({index});", gameObject);
            return null;
        }

        StoredItem? storeditem = items[index];

        if (storeditem != null)
        {
            items[index].itemID = null;
            items[index].count = 0;
        }
        OnInventoryChanged?.Invoke();
        return storeditem?.itemID;
    }

    public bool HasItem(string itemID)
    {
        return items.Any(storeditem => storeditem.itemID == itemID);
    }

    /// <summary>
    /// Returns the index of the first empty slot or -1 if none exist
    /// </summary>
    /// <returns></returns>
    public int FindEmptySlot()
    {
        for (int i = 0; i < capacity; i++)
        {
            if (items[i].itemID == null)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the first slot with the specfied item or -1 if none exist
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public int FindItemSlot(string itemID)
    {
        return Array.FindIndex(items, item => item.itemID == itemID);
    }

    /// <summary>
    /// Returns the first none-full slot of the specified item or -1 if none exist
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public int FindItemSlotWithSpace(string itemID)
    {
        for(int i = 0; i < capacity; i++)
        {
            if (items[i].itemID == itemID && CapacityLeft(i) > 0)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Returns the first none-full slot of the specified item, or an empty slot, or -1 if neither exist
    /// </summary>
    /// <param name="itemID"></param>
    /// <returns></returns>
    public int FindItemSlotWithSpaceOrEmpty(string itemID)
    {
        int slot = FindItemSlotWithSpace(itemID);

        if (slot < 0)
        {
            slot = FindEmptySlot();
        }

        return slot;
    }

    public bool IsSlotEmpty(int index)
    {
        if (index >= capacity || index < 0)
        {
            Debug.LogError($"ERROR: Invalid attempt to access inventory (capacity {capacity}) at slot ({index});", gameObject);
            return false;
        }

        return items[index].itemID == null;
    }

    public int CapacityLeft(int index)
    {
        if (index >= capacity || index < 0)
        {
            Debug.LogError($"ERROR: Invalid attempt to access inventory (capacity {capacity}) at slot ({index});", gameObject);
            return 0;
        }

        var max = itemDB.GetItemByID(items[index].itemID).maxStackSize;

        return max - items[index].count;
    }
}
