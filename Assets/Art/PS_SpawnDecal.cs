using UnityEngine;

public class ParticleDecalSpawner2D : MonoBehaviour
{
    [Header("Decal Settings")]
    public GameObject decalPrefab;
    [Tooltip("Set to 0 or less to make decals last forever.")]
    public float decalLifetime = 0f;
    public float zOffset = -0.01f;

    [Header("Spawn Control")]
    [Tooltip("Minimum time between decal spawns (in seconds).")]
    public float spawnCooldown = 0.1f;

    [Header("Random Transform")]
    public Vector2 randomScaleRange = new Vector2(0.8f, 1.2f);
    public bool randomRotation = true;

    private ParticleSystem ps;
    private float lastSpawnTime = -Mathf.Infinity;

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

        if (eventCount == 0)
            return;

        // Pick a random collision to use (adds variety)
        ParticleCollisionEvent e = collisionEvents[Random.Range(0, eventCount)];
        Vector3 spawnPos = new Vector3(e.intersection.x, e.intersection.y, zOffset);

        Quaternion rotation = randomRotation
            ? Quaternion.Euler(0f, 0f, Random.Range(0f, 360f))
            : Quaternion.identity;

        GameObject decal = Instantiate(decalPrefab, spawnPos, rotation);

        float randomScale = Random.Range(randomScaleRange.x, randomScaleRange.y);
        decal.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        if (decalLifetime > 0f)
        {
            Destroy(decal, decalLifetime);
        }

        // Add fading component if not present
        if (!decal.GetComponent<DecalFader>())
        {
            decal.AddComponent<DecalFader>();
        }

        lastSpawnTime = Time.time;
    }
}
