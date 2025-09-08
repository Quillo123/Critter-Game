using UnityEngine;

public class SimplePhysicsBody : MonoBehaviour
{
    public float mass = 1;
    public float resistance = 1;
    public bool simulate = true;


    Vector2 velocity = Vector2.zero;
    bool isMoving = false;

    private void Update()
    {

        if (simulate && isMoving)
        {
            if (velocity.x < 0.01 && velocity.y < 0.01)
            {
                isMoving = false;
                velocity = Vector2.zero;
            }

            transform.position += velocity.ToVector3() * Time.deltaTime;
            velocity -= velocity * resistance * Time.deltaTime;
        }
    }

    public void AddForce(Vector2 force)
    {
        isMoving = true;
        velocity = force / mass;
    }
}
