using NUnit.Framework;
using System.Collections;
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

    private bool destroying = false;

    public float fadeDuration = 1;

    SpriteRenderer sr;

    public Material defaultMat;
    public Material highlight;
    public Material dissolve;


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere((Vector2)transform.position + spawnOffset, 0.5f);
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if(defaultMat == null)
        {
            defaultMat = sr.sharedMaterial;
        }
        else
        {
            sr.sharedMaterial = defaultMat;
        }
    }

    private void OnMouseEnter()
    {
        if (!destroying)
        {
            sr.sharedMaterial = highlight;
        }
    }

    private void OnMouseExit()
    {
        if (!destroying)
        {
            sr.sharedMaterial = defaultMat;
        }
    }

    private void OnMouseOver()
    { 
        if(Input.GetMouseButton(0) && !destroying)
        {
            if(startTime == 0) startTime = Time.time;
            if(Time.time - startTime >= breakTime)
            {
                StartCoroutine(DropItemsAndDestroy());
            }
        }
        else
        {
            startTime = 0;
        }
    }

    private IEnumerator DropItemsAndDestroy()
    {
        destroying = true;
        sr.sharedMaterial = dissolve;

        float elapsedTime = 0f;


        float dropwait = (fadeDuration * .5f) / drops.Count;
        float droptime = dropwait;
        int i = 0;


        while (elapsedTime < fadeDuration || i < drops.Count)
        {
            elapsedTime += Time.deltaTime;
            float fade = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            sr.material.SetFloat("_Fade", fade);

            if (elapsedTime > droptime && i < drops.Count)
            {
                SpawnDrop(drops[i]);
                droptime += dropwait;
                i++;
            }

            yield return null; // Wait for the next frame
        }

        Destroy(gameObject);
    }

    void SpawnDrop(Item drop)
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



}

