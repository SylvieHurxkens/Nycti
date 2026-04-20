using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerPro : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f; // 
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("References")]
    public CharacterController controller;

    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSprinting;

    // Wordt aangeroepen door de Move actie
    public void OnMove(InputValue context)
    {
        moveInput = context.Get<Vector2>();
    }

    // Wordt aangeroepen door de Sprint actie
    public void OnSprint(InputValue context)
    {
        isSprinting = context.isPressed;

    }

    public void OnJump(InputValue context)
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        // Bepaal de huidige snelheid
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        // Beweging berekenen
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        controller.Move(move * currentSpeed * Time.deltaTime);

        // Zwaartekracht
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}