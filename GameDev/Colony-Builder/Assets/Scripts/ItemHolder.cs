using UnityEngine;

public enum Direction { Down, Up, Left, Right } // Add more if needed, e.g., diagonals

public class ItemHolder : MonoBehaviour
{
    [SerializeField] public Transform equippedItem; // Reference to the item's Transform
    [SerializeField] private Vector2 offsetDown = new Vector2(0.5f, 0f); // Example defaults
    [SerializeField] private Vector2 offsetUp = new Vector2(0.5f, 0.5f);
    [SerializeField] private Vector2 offsetLeft = new Vector2(-0.5f, 0f);
    [SerializeField] private Vector2 offsetRight = new Vector2(0.5f, 0f);

    [SerializeField]
    private Direction currentDirection = Direction.Down; // Track current direction

    private bool equipped = false;

    private void OnDrawGizmosSelected()
    {
        // Approach 1: Draw offsets as spheres
        Gizmos.color = Color.green;
        switch (currentDirection)
        {
            case Direction.Down:
                Gizmos.DrawWireSphere(transform.position + (Vector3)offsetDown, 0.1f);
                break;
            case Direction.Up:
                Gizmos.DrawWireSphere(transform.position + (Vector3)offsetUp, 0.1f);
                break;
            case Direction.Left:
                Gizmos.DrawWireSphere(transform.position + (Vector3)offsetLeft, 0.1f);
                break;
            case Direction.Right:
                Gizmos.DrawWireSphere(transform.position + (Vector3)offsetRight, 0.1f);
                break;
        }        
    }

    public void SwapEquippedItem(Item item)
    {
        equippedItem.gameObject.SetActive(false);

        equippedItem = item.transform;
        equippedItem.gameObject.SetActive(equipped);
        UpdateHoldPosition(currentDirection); // Reposition based on current direction
    }
    
    public void UpdateEquipped(bool isEquipped)
    {
        equipped = isEquipped;
        equippedItem.gameObject.SetActive(equipped);
    }

    // Call this when direction changes (e.g., from input or animator)
    public void UpdateHoldPosition(Direction newDirection)
    {
        currentDirection = newDirection;
        Vector2 offset = GetOffsetForDirection();

        equippedItem.localPosition = new Vector3(offset.x, offset.y, equippedItem.localPosition.z );

        // Handle flipping for left/right if your character doesn't auto-flip the item
        if (currentDirection == Direction.Left)
        {
            equippedItem.localScale = new Vector2(-1f, 1f); // Flip item sprite horizontally
        }
        else if (currentDirection == Direction.Right)
        {
            equippedItem.localScale = new Vector2(1f, 1f); // Reset flip
        }
    }

    private Vector2 GetOffsetForDirection()
    {
        switch (currentDirection)
        {
            case Direction.Down: return offsetDown;
            case Direction.Up: return offsetUp;
            case Direction.Left: return offsetLeft;
            case Direction.Right: return offsetRight;
            default: return Vector2.zero;
        }
    }
}