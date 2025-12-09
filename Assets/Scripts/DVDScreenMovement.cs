using UnityEngine;

/// <summary>
/// Moves a GameObject in a DVD screensaver-like pattern, bouncing off boundaries
/// with perfect reflection.
/// </summary>
public class DVDScreenMovement : MonoBehaviour
{
    public float Speed { get; set; } = 5f;

    public Bounds Bounds { get; set; }

    private Vector3 velocity;

    void Start()
    {
        // Initialize with a random direction (normalized) and apply speed
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        velocity = (Vector3)randomDirection * Speed;
    }

    void Update()
    {
        // Move the object
        transform.position += velocity * Time.deltaTime;

        // Check bounds and reflect if necessary
        Vector3 position = transform.position;
        Vector3 reflectedPosition = position;
        bool reflected = false;

        float minX = Bounds.center.x - Bounds.extents.x;
        float maxX = Bounds.center.x + Bounds.extents.x;
        float minY = Bounds.center.y - Bounds.extents.y;
        float maxY = Bounds.center.y + Bounds.extents.y;

        // Check X bounds
        if (position.x < minX)
        {
            reflectedPosition.x = minX;
            velocity.x = Mathf.Abs(velocity.x); // Reflect to positive direction
            reflected = true;
        }
        else if (position.x > maxX)
        {
            reflectedPosition.x = maxX;
            velocity.x = -Mathf.Abs(velocity.x); // Reflect to negative direction
            reflected = true;
        }

        // Check Y bounds
        if (position.y < minY)
        {
            reflectedPosition.y = minY;
            velocity.y = Mathf.Abs(velocity.y); // Reflect to positive direction
            reflected = true;
        }
        else if (position.y > maxY)
        {
            reflectedPosition.y = maxY;
            velocity.y = -Mathf.Abs(velocity.y); // Reflect to negative direction
            reflected = true;
        }

        // Update position if reflection occurred
        if (reflected)
        {
            transform.position = reflectedPosition;
        }
    }
}
