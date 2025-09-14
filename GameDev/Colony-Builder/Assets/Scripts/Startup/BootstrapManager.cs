using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BootstrapManager : MonoBehaviour
{
    [SerializeField] private string mainSceneName = "MainGameScene"; // Set in Inspector

    void Start()
    {
        // Start the initialization process
        StartCoroutine(InitializeGame());
    }

    IEnumerator InitializeGame()
    {
        // Wait for ItemDatabase to initialize
        yield return StartCoroutine(InitializeItemDatabase());

        // Load the main game scene
        SceneManager.LoadScene(mainSceneName);
    }

    IEnumerator InitializeItemDatabase()
    {
        // Ensure ItemDatabase singleton is initialized
        if (ItemDatabase.Instance == null)
        {
            // Find or create ItemDatabase GameObject
            GameObject itemDatabaseObj = GameObject.Find("ItemDatabase");
            if (itemDatabaseObj == null)
            {
                itemDatabaseObj = new GameObject("ItemDatabase");
                itemDatabaseObj.AddComponent<ItemDatabase>();
            }

            // Wait until ItemDatabase is ready
            while (ItemDatabase.Instance == null || ItemDatabase.Instance.items == null || ItemDatabase.Instance.items.Count == 0)
            {
                yield return null; // Wait for the next frame
            }

            Debug.Log("ItemDatabase initialized with " + ItemDatabase.Instance.items.Count + " items.");
        }
    }
}