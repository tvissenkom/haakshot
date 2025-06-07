using UnityEngine;

public class PhysicsGrapple : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float hookSpeed = 20f;
    public float maxHookDistance = 10f;
    public float pullForce = 30f;
    public LayerMask hookableLayers;
    public float stopPullDistance = 1f;

    private Vector2 hookTarget;
    private bool isHookFlying = false;
    private bool isPulling = false;
    private bool hookHitSomething = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireHook();
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
                    isPulling = true;
                }
                else
                {
                    CancelHook();
                }
            }
        }

        if (isPulling)
        {
            Vector2 direction = (hookTarget - rb.position).normalized;
            float distance = Vector2.Distance(rb.position, hookTarget);

            if (distance < stopPullDistance)
            {
                CancelHook();
                return;
            }

            rb.AddForce(direction * pullForce);

            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hookTarget);
        }
    }

    private void FireHook()
    {
        CancelHook();

        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorld - rb.position).normalized;
        Vector2 maxPoint = rb.position + dir * maxHookDistance;

        RaycastHit2D hit = Physics2D.Raycast(rb.position, dir, maxHookDistance, hookableLayers);

        if (hit.collider != null)
        {
            hookTarget = hit.point;
            hookHitSomething = true;
        }
        else
        {
            hookTarget = maxPoint;
            hookHitSomething = false;
        }

        isHookFlying = true;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void CancelHook()
    {
        isHookFlying = false;
        isPulling = false;
        hookHitSomething = false;
        lineRenderer.enabled = false;
    }
}
