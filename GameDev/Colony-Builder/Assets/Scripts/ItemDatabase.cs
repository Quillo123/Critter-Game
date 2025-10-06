using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    // Singleton instance
    public static ItemDatabase Instance { get; private set; }

    public Dictionary<string, Item> items = new Dictionary<string, Item>();
    public ItemEntity itemPrefab;

    void Awake()
    {
        // Singleton setup: Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            LoadItems();
            LoadPrefabs();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }
    }

    private void OnDestroy()
    {
        if(this == Instance)
        {
            Instance = null;
        }
    }

    void LoadPrefabs()
    {
        itemPrefab = Resources.Load<ItemEntity>("Prefabs/Item");
        if(itemPrefab == null)
        {
            Logger.LogError($"Could not load default item prefab.", gameObject);
            return;
        }
    }

    void LoadItems()
    {
        // Load all Item ScriptableObjects from the "Items" folder in Resources
        Item[] loadedItems = Resources.LoadAll<Item>("Items");

        // Check for duplicate or invalid itemIDs
        var duplicateIDs = loadedItems
            .GroupBy(item => item.itemID)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        var invalidIDs = loadedItems
            .Where(item => string.IsNullOrEmpty(item.itemID))
            .Select(item => item.name)
            .ToList();

        if (duplicateIDs.Count > 0)
        {
            Logger.LogError($"Duplicate item IDs found: {string.Join(", ", duplicateIDs)}. Each Item must have a unique itemID.", gameObject);
            return;
        }

        if (invalidIDs.Count > 0)
        {
            Logger.LogError($"Invalid (null or empty) item IDs found in assets: {string.Join(", ", invalidIDs)}. Each Item must have a valid itemID.", gameObject);
            return;
        }

        // Populate the dictionary
        foreach (Item item in loadedItems)
        {
            items.Add(item.itemID, item);
        }

        Logger.Log($"Loaded {items.Count} items.", gameObject);
    }

    public Item GetItemByID(string itemID)
    {
        if (string.IsNullOrEmpty(itemID))
        {
            Logger.LogWarning("GetItemByID called with null or empty itemID.", gameObject);
            return null;
        }

        return items.TryGetValue(itemID, out Item item) ? item : null;
    }

    public Item GetItemByName(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Logger.LogWarning("GetItemByName called with null or empty itemName.", gameObject);
            return null;
        }

        return items.Values.FirstOrDefault(item => item.itemName == itemName);
    }

}