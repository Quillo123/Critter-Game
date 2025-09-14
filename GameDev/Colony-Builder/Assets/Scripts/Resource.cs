using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class Resource : MonoBehaviour
{
    public float breakTime = 2f;

    private float startTime = 0;

    public List<Item> drops;

    public Vector2 spawnOffset = Vector2.zero;

    public float spawnForce = 1;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + spawnOffset, 0.5f);
    }

    private void OnMouseOver()
    {
        if(Input.GetMouseButton(0))
        {
            if(startTime == 0) startTime = Time.time;
            if(Time.time - startTime >= breakTime)
            {
                DropItemsAndDestroy();
            }
        }
        else
        {
            startTime = 0;
        }
    }

    private void DropItemsAndDestroy()
    {
        foreach (Item drop in drops)
        {
            var spawnpos = transform.position + new Vector3(spawnOffset.x, spawnOffset.y, 0f);
            var itemE = Instantiate(ItemDatabase.Instance.itemPrefab, spawnpos, Quaternion.identity);
            itemE.item = drop;

            var itemRB = itemE.GetComponent<Rigidbody2D>();
            if (itemRB != null)
            {
                itemRB.AddForce(Random.insideUnitCircle * spawnForce);
            }
        }
        Destroy(gameObject);
    }
}

