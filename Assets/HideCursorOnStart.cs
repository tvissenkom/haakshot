using UnityEngine;

public class HideCursorOnStart : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // Keeps it within the game window
    }
}