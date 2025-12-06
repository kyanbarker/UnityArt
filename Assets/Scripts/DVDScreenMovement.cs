using UnityEngine;

/// <summary>
/// Moves a GameObject in a DVD screensaver-like pattern, bouncing off boundaries
/// with perfect reflection.
/// </summary>
public class DVDScreenMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    [SerializeField]
    private Bounds bounds = new Bounds(Vector3.zero, new Vector3(40f, 20f, 0f));

    public Bounds Bounds
    {
        get => bounds;
        set => bounds = value;
    }

    private Vector3 velocity;

    void Start()
    {
        // Initialize with a random direction (normalized) and apply speed
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        velocity = (Vector3)randomDirection * speed;
    }

    void Update()
    {
        // Move the object
        transform.position += velocity * Time.deltaTime;

        // Check bounds and reflect if necessary
        Vector3 position = transform.position;
        Vector3 reflectedPosition = position;
        bool reflected = false;

        float minX = bounds.center.x - bounds.extents.x;
        float maxX = bounds.center.x + bounds.extents.x;
        float minY = bounds.center.y - bounds.extents.y;
        float maxY = bounds.center.y + bounds.extents.y;

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
