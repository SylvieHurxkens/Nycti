using UnityEngine;
using UnityEngine.InputSystem; // Belangrijk!

public class MouseLookPro : MonoBehaviour
{
    public float mouseSensitivity = 25f;
    public Transform cameraTransform;
    private float xRotation = 0f;
    private Vector2 lookInput;

    void Start()
{
    // Verbergt de cursor en zet hem vast in het midden van het scherm
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
}

    // Deze methode koppel je aan de "Look" actie in je Player Input component
    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Update()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}