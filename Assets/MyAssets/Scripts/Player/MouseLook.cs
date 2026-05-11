using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 2f;
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
    public void OnLook(InputValue context)
    {
        lookInput = context.Get<Vector2>();
    }

    void Update()
    {
        if (Time.timeScale == 0) 
        {
            return; 
        }

        //Debug.Log("Huidige Sens: " + SettingsManager.mouseSensitivity);

        float currentSens = SettingsManager.mouseSensitivity;

        float mouseX = lookInput.x * mouseSensitivity * currentSens * Time.deltaTime * 50f;
        float mouseY = lookInput.y * mouseSensitivity * currentSens * Time.deltaTime * 50f;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}