using UnityEngine;

public class ScreenShakeController : MonoBehaviour
{
    public static ScreenShakeController Instance;

    [Header("Shake Settings")]
    public float shakeIntensity = 0.5f;
    public float shakeDuration = 0.5f;

    private Transform camTransform;
    private Vector3 originalPos;
    private float shakeTimer = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (Camera.main == null)
        {
            Debug.LogError("No main camera found in the scene!");
            enabled = false;
            return;
        }

        camTransform = Camera.main.transform;
        originalPos = camTransform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0f)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeIntensity;

            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0f)
            {
                camTransform.localPosition = originalPos;
            }
        }
    }

    /// <summary>
    /// Triggers a camera shake.
    /// </summary>
    /// <param name="intensity">Shake strength.</param>
    /// <param name="duration">Shake duration in seconds.</param>
    public void Shake(float intensity, float duration)
    {
        shakeIntensity = intensity;
        shakeDuration = duration;
        shakeTimer = duration;
    }
}