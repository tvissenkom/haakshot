using UnityEngine;

public class CursorTarget2D : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 mouseScreen = Input.mousePosition;
        Vector3 world = cam.ScreenToWorldPoint(mouseScreen);
        world.z = 0f; // Lock to 2D plane
        transform.position = world;
    }
}
