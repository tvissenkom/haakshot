using UnityEngine;

public class KillOnHighImpactCollision : MonoBehaviour
{
    [Tooltip("Minimum collision velocity magnitude to trigger death")]
    public float velocityThreshold = 5f;

    // Called when this collider/rigidbody has begun touching another rigidbody/collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check the relative velocity magnitude of the collision
        if (collision.relativeVelocity.magnitude > velocityThreshold)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        Debug.Log("Player killed due to high impact collision!");

        // Example 1: Destroy the player GameObject
        Destroy(gameObject);

        // Example 2: Alternatively, you could disable controls, play death animation, etc.
        // For example: GetComponent<PlayerController>().enabled = false;
        // Or invoke a death event if you have one
    }
}