using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct ResourceSpawn
    {
        public Resource resource;

        [Range(0,1)]
        public float spawnChance;
    }

    public List<ResourceSpawn> resources;
    float spawnrate = 1;

    public Rect spawnArea;

    private void OnDrawGizmosSelected()
    {
        if (spawnArea == Rect.zero) return; // Skip if not set

        // Set color for the gizmo (semi-transparent green)
        Gizmos.color = new Color(0f, 1f, 0f, 0.5f); // Green with alpha

        // Calculate 3D center and size (assuming Z=0 plane)
        Vector3 center = new Vector3(spawnArea.center.x, spawnArea.center.y, 0f);
        Vector3 size = new Vector3(spawnArea.width, spawnArea.height, 0.1f); // Thin depth for visibility

        // Draw wireframe cube (outlines the Rect)
        Gizmos.DrawWireCube(center, size);

        // Optional: Draw a solid cube for filled visualization (semi-transparent)
        // Gizmos.DrawCube(center, size);
    }

    private void Start()
    {
        StartCoroutine(SpawnResources());
    }

    IEnumerator SpawnResources()
    {
        while (true)
        {
            foreach(ResourceSpawn resource in resources)
            {
                SpawnResource(resource);
            }

            yield return new WaitForSeconds(spawnrate);
        }
    }

    void SpawnResource(ResourceSpawn resource)
    {
        if(Random.value <= resource.spawnChance)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(spawnArea.xMin, spawnArea.xMax),
                Random.Range(spawnArea.yMin, spawnArea.yMax)
            );

            Instantiate(resource.resource, randomPos, Quaternion.identity);
        }
    }
}
