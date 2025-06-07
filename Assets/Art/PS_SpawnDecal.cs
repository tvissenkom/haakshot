using UnityEngine;

public class ParticleDecalSpawner2D : MonoBehaviour
{
    [Header("Decal Settings")]
    public GameObject decalPrefab;
    public float decalLifetime = 5f;
    public float zOffset = -0.01f;

    [Header("Spawn Control")]
    [Tooltip("Minimum time between decal spawns (in seconds).")]
    public float spawnCooldown = 0.2f;

    [Header("Random Transform")]
    public Vector2 randomScaleRange = new Vector2(0.8f, 1.2f);
    public bool randomRotation = true;

    private float lastSpawnTime = -Mathf.Infinity;
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (Time.time - lastSpawnTime < spawnCooldown)
            return;

        ParticleCollisionEvent[] collisionEvents = new ParticleCollisionEvent[16];
        int eventCount = ps.GetCollisionEvents(other, collisionEvents);

        if (eventCount > 0)
        {
            // Use only the first event for now (to throttle per frame)
            ParticleCollisionEvent e = collisionEvents[0];
            Vector3 spawnPos = new Vector3(e.intersection.x, e.intersection.y, zOffset);

            // Random rotation (Z-axis only for 2D)
            Quaternion rotation = randomRotation
                ? Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))
                : Quaternion.identity;

            GameObject decal = Instantiate(decalPrefab, spawnPos, rotation);

            // Apply random scale
            float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
            decal.transform.localScale = new Vector3(randomScale, randomScale, 1f);

            Destroy(decal, decalLifetime);
            lastSpawnTime = Time.time;
        }
    }
}
