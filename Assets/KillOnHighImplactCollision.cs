using UnityEngine;

public class KillOnHighImpactCollision : MonoBehaviour
{
    [Tooltip("Minimum collision velocity magnitude to trigger death")]
    public float velocityThreshold = 5f;

    [Tooltip("Blood particle prefab to spawn on death")]
    public GameObject bloodParticles;

    private Vector3 startPosition;
    private PhysicsGrapple hookshotController;
    private ScreenShakeController screenShakeController;

    private void Awake()
    {
        hookshotController = GetComponent<PhysicsGrapple>();
        screenShakeController = GetComponent<ScreenShakeController>();
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.relativeVelocity.magnitude > velocityThreshold)
        {
            // Get direction of impact
            Vector2 impactDirection = collision.relativeVelocity.normalized;

            screenShakeController.Shake(5,1);

            // Instantiate blood particles with opposite rotation
            Quaternion bloodRotation = Quaternion.FromToRotation(Vector2.right, -impactDirection);
            Instantiate(bloodParticles, transform.position, bloodRotation);

            //Reset sidersilk
            hookshotController.CancelHook();

            // Reset player position or handle death logic
            Debug.Log("Player killed due to high impact collision!");
            transform.position = startPosition;
        }
    }
}