using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ItemMagnet : MonoBehaviour
{
    public float attractionForce = 10f;
    [Range(0.01f, 2f)]
    public float minDistance = 0.1f;

    public bool IsPickupMagnet = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        var item = collision.GetComponent<ItemEntity>();
        if (item && (!IsPickupMagnet || item.CanPickUp()))
        {
            Vector2 dir = transform.position - collision.transform.position;
            float distance = dir.magnitude;

            if (distance > minDistance)
            {
                Vector2 force = (attractionForce / distance) * dir.normalized; // Inverse distance for realism
                collision.GetComponent<Rigidbody2D>().AddForce(force);
            }
        }
    }
}
