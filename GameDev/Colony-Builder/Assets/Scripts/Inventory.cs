using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemEntity> items;
    public int capacity = 20;

    public bool AddItem(ItemEntity item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory full");
            return false;
        }
        items.Add(item);
        return true;
    }

    public bool RemoveItem(ItemEntity item)
    {
        return items.Remove(item);
    }

    public bool HasItem(ItemEntity item)
    {
        return items.Contains(item);
    }
}
