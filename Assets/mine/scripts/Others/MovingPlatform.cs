using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform PointA; // Start position
    public Transform PointB; // End position
    public float speed = 2.0f; // Platform movement speed

    private Vector3 nextPosition; // Next target position for movement
    private Vector3 initialPosition; // Store platform's initial position for reset

    void Start()
    {
        initialPosition = transform.position; // Save initial position
        nextPosition = PointB.position; // Start moving towards PointB
    }

    void Update()
    {
        // Move platform smoothly towards the target
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, speed * Time.deltaTime);

        // Switch direction when reaching target
        if (transform.position == nextPosition)
        {
            nextPosition = (nextPosition == PointA.position) ? PointB.position : PointA.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Parent player to platform when standing on it
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.transform.parent = transform;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            collision.gameObject.transform.parent = null;
        }
    }


    // Reset platform to its starting position (used on level reset)
    public void ResetPlatform()
    {
        transform.position = initialPosition;
        nextPosition = PointB.position;
    }
}



