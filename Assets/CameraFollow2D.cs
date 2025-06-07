using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow2D : MonoBehaviour
{
    [Header("Target")]
    public Transform player;
    public Transform cursorTarget; // Optional: world-space cursor object

    [Header("Follow Settings")]
    public float followSpeed = 5f;
    public Vector2 offset = Vector2.zero;

    [Header("Cursor Influence")]
    [Range(0f, 1f)]
    public float cursorWeight = 0.3f;

    [Header("Dead Zone")]
    public bool useDeadZone = true;
    public Vector2 deadZone = new Vector2(0.5f, 0.3f);

    private Vector3 currentVelocity;

    void LateUpdate()
    {
        if (player == null)
            return;

        // Base position is player
        Vector3 targetPos = player.position;

        // Add offset
        Vector3 finalTarget = targetPos + (Vector3)offset;

        // Blend toward cursor
        if (cursorTarget != null)
        {
            Vector3 cursorPos = cursorTarget.position;
            finalTarget = Vector3.Lerp(targetPos, cursorPos, cursorWeight);
        }

        // Keep current Z
        finalTarget.z = transform.position.z;

        // Dead zone (optional)
        if (useDeadZone)
        {
            Vector3 diff = finalTarget - transform.position;
            if (Mathf.Abs(diff.x) < deadZone.x) finalTarget.x = transform.position.x;
            if (Mathf.Abs(diff.y) < deadZone.y) finalTarget.y = transform.position.y;
        }

        // Smooth movement
        transform.position = Vector3.SmoothDamp(transform.position, finalTarget, ref currentVelocity, 1f / followSpeed);
    }
}
