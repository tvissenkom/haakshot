using UnityEngine;

public class HookshotController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float hookSpeed = 20f;
    public float pullSpeed = 10f;
    public LayerMask hookableLayers;

    private Vector2 hookTarget;
    private bool isHookFlying = false;
    private bool isPullingPlayer = false;
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
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, mouseWorldPos - (Vector2)transform.position, Mathf.Infinity, hookableLayers);

            if (hit.collider != null)
            {
                hookTarget = hit.point;
                isHookFlying = true;
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position); // Start at player
            }
        }

        if (isHookFlying)
        {
            // Animate line towards hook target
            Vector2 currentEnd = Vector2.MoveTowards(lineRenderer.GetPosition(1), hookTarget, hookSpeed * Time.deltaTime);
            lineRenderer.SetPosition(1, currentEnd);

            if (Vector2.Distance(currentEnd, hookTarget) < 0.1f)
            {
                isHookFlying = false;
                isPullingPlayer = true;
            }
        }

        if (isPullingPlayer)
        {
            rb.MovePosition(Vector2.MoveTowards(rb.position, hookTarget, pullSpeed * Time.deltaTime));
            lineRenderer.SetPosition(0, transform.position); // Update start to player

            if (Vector2.Distance(rb.position, hookTarget) < 0.5f)
            {
                isPullingPlayer = false;
                lineRenderer.enabled = false;
            }
        }
    }
}
