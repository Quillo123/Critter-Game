
using log4net.Util;
using QFSW.QC;
using UnityEngine;


public class Commands
{

    [Command]
    static void SpawnItem(string itemID, Vector2 position)
    {
        if (string.IsNullOrEmpty(itemID))
        {
            Logger.LogError("No item ID was supplied", null);
            return;
        }

        var spawnpos = position.ToVector3();
        var itemE = GameObject.Instantiate(ItemDatabase.Instance.itemPrefab, spawnpos, Quaternion.identity);
    }


}
