using UnityEngine;

public class DecalFader : MonoBehaviour
{
    public float darkenSpeed = 0.05f; // how fast to darken
    public float targetBrightness = 0.3f; // final color brightness

    private SpriteRenderer sr;
    private Color originalColor;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
        else
        {
            Debug.LogWarning("DecalFader: No SpriteRenderer found!");
            enabled = false;
        }
    }

    private void Update()
    {
        if (sr == null) return;

        Color current = sr.color;
        float currentBrightness = (current.r + current.g + current.b) / 3f;

        if (currentBrightness > targetBrightness)
        {
            Color darker = Color.Lerp(current, Color.black, darkenSpeed * Time.deltaTime);
            sr.color = darker;
        }
    }
}
