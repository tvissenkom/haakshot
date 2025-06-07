using UnityEngine;

public class HookshotController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float hookSpeed = 20f;
    public float pullSpeed = 10f;
    public float maxHookDistance = 10f;
    public LayerMask hookableLayers;

    private Vector2 hookTarget;
    private bool isHookFlying = false;
    private bool isPullingPlayer = false;
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
            // Cancel any existing hook or pull
            CancelHook();

            // Start a new hookshot
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;
            Vector2 maxPoint = (Vector2)transform.position + direction * maxHookDistance;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, maxHookDistance, hookableLayers);

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

        if (isHookFlying)
        {
            Vector2 currentEnd = Vector2.MoveTowards(lineRenderer.GetPosition(1), hookTarget, hookSpeed * Time.deltaTime);
            lineRenderer.SetPosition(1, currentEnd);

            if (Vector2.Distance(currentEnd, hookTarget) < 0.1f)
            {
                isHookFlying = false;

                if (hookHitSomething)
                {
                    isPullingPlayer = true;
                }
                else
                {
                    lineRenderer.enabled = false;
                }
            }
        }

        if (isPullingPlayer)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, hookTarget, pullSpeed * Time.deltaTime));
            lineRenderer.SetPosition(0, transform.position);

            if (Vector2.Distance(rb.position, hookTarget) < 0.5f)
            {
                isPullingPlayer = false;
                lineRenderer.enabled = false;
            }
        }
    }

    private void CancelHook()
    {
        isHookFlying = false;
        isPullingPlayer = false;
        hookHitSomething = false;
        lineRenderer.enabled = false;
    }
}
