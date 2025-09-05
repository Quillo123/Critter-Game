using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public int capacity = 20;

    public bool AddItem(Item item)
    {
        if (items.Count >= capacity)
        {
            Debug.Log("Inventory full");
            return false;
        }
        items.Add(item);
        return true;
    }

    public bool RemoveItem(Item item)
    {
        return items.Remove(item);
    }

    public bool HasItem(Item item)
    {
        return items.Contains(item);
    }
}
