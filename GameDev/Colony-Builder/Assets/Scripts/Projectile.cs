using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 velocity = Vector2.zero;
    public float lifetime = 1;
    public float deceleration = 0;

    float startTime = 0;
    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {

        if(Time.time - startTime > lifetime)
        {
            Destroy(gameObject);
        }

        transform.position += velocity.ToVector3() * Time.deltaTime;
        velocity -= velocity.normalized * deceleration * Time.deltaTime;
    }
}
