using UnityEngine;

public class PhysicsGrapple : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float hookSpeed = 20f;
    public float maxHookDistance = 10f;
    public float pullForce = 30f;
    public float objectPullForce = 20f;
    public float stopPullDistance = 1f;

    // New variables for acceleration
    public float pullAcceleration = 10f;
    public float maxPullForce = 100f;

    public LayerMask hookableLayers;
    public LayerMask unhookableLayers;

    private Vector2 hookTarget;
    private bool isHookFlying = false;
    private bool isPulling = false;
    private bool hookHitSomething = false;
    private bool grabbedUnhookable = false;

    private Rigidbody2D rb;
    private Rigidbody2D hookedRigidbody; // Target being pulled

    // Variable to track the current force
    private float currentPullForce;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isHookFlying) // Prevent re-firing while already pulling
            {
                FireHook();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelHook();
        }

        if (isHookFlying)
        {
            Vector2 currentEnd = Vector2.MoveTowards(lineRenderer.GetPosition(1), hookTarget, hookSpeed * Time.deltaTime);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, currentEnd);

            if (Vector2.Distance(currentEnd, hookTarget) < 0.1f)
            {
                isHookFlying = false;

                if (hookHitSomething)
                {
                    if (!grabbedUnhookable)
                    {
                        isPulling = true;
                        // Reset the pull force each time we start pulling
                        currentPullForce = pullForce;
                    }
                    else
                    {
                        grabbedUnhookable = false;
                        CancelHook();
                    }
                }
                else
                {
                    CancelHook();
                }
            }
        }

        if (isPulling)
        {
            // Check if the pull button is being held to apply acceleration
            if (Input.GetMouseButton(0))
            {
                // Increase the pull force over time
                currentPullForce += pullAcceleration * Time.deltaTime;
                currentPullForce = Mathf.Min(currentPullForce, maxPullForce); // Clamp to the max force
            }

            if (hookedRigidbody != null)
            {
                // Pull the object toward the player
                Vector2 dir = (rb.position - hookedRigidbody.position).normalized;
                float distance = Vector2.Distance(rb.position, hookedRigidbody.position);

                if (distance < stopPullDistance)
                {
                    CancelHook();
                    return;
                }

                // Apply the current (accelerated) force
                hookedRigidbody.AddForce(dir * currentPullForce);
                lineRenderer.SetPosition(0, rb.position);
                lineRenderer.SetPosition(1, hookedRigidbody.position);
            }
            else
            {
                // Pull the player toward the static point
                Vector2 dir = (hookTarget - rb.position).normalized;
                float distance = Vector2.Distance(rb.position, hookTarget);

                if (distance < stopPullDistance)
                {
                    CancelHook();
                    return;
                }

                // Apply the current (accelerated) force
                rb.AddForce(dir * currentPullForce);
                lineRenderer.SetPosition(0, rb.position);
                lineRenderer.SetPosition(1, hookTarget);
            }
        }
    }

    private void FireHook()
    {
        CancelHook();

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorld - rb.position).normalized;

        // Combine the two masks so we hit both hookable and unhookable
        LayerMask combinedMask = hookableLayers | unhookableLayers;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, dir, maxHookDistance, combinedMask);

        if (hit.collider != null)
        {
            // Check if hit is on an unhookable layer
            if (((1 << hit.collider.gameObject.layer) & unhookableLayers) != 0)
            {
                // Immediately cancel the hook if hit unhookable
                grabbedUnhookable = true;
            }

            // Else, normal hookable logic
            hookTarget = hit.point;
            hookHitSomething = true;

            // Check if the hit object has a rigidbody
            hookedRigidbody = hit.collider.attachedRigidbody;
        }
        else
        {
            hookTarget = rb.position + dir * maxHookDistance;
            hookHitSomething = false;
            hookedRigidbody = null;
        }

        isHookFlying = true;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, rb.position);
        lineRenderer.SetPosition(1, rb.position);
    }

    public void CancelHook()
    {
        isHookFlying = false;
        isPulling = false;
        hookHitSomething = false;
        hookedRigidbody = null;
        lineRenderer.enabled = false;
    }
}