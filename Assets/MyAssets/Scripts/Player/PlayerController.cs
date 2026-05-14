using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f; // 
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    [Header("Interaction")]
    public float interactRange = 3f; // Hoe ver je de bel kunt raken

    [Header("References")]
    public CharacterController controller;
    public Camera playerCamera; 

    private Vector2 moveInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSprinting;

    // Dit geeft de FootstepGenerator toestemming om te zien of isSprinting aan staat
    public bool IsSprintingPublic => isSprinting;
    public Vector2 CurrentMoveInput => moveInput;

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

    public void OnInteract(InputValue context)
    {
        if (context.isPressed)
        {
            // Schiet een straal vanuit het midden van de camera
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactRange))
            {
                // Zoek naar het script op de bel (of ander interactable object)
                var bell = hit.transform.GetComponent<BellInteraction>();
                
                if (bell != null)
                {
                    bell.OnInteract();
                }
            }
        }
    }

    void Start()
    {
        // Check of we net uit een scène komen waar de bel geluid is
        if (BellInteraction.moetPositieHerstellen)
        {
            // Zet de speler op de oude plek
            // Let op: als je een CharacterController gebruikt, zet deze dan even uit
            if (controller != null) controller.enabled = false;
            
            transform.position = BellInteraction.opgeslagenPositie;
            transform.rotation = BellInteraction.opgeslagenRotatie;
            
            if (controller != null) controller.enabled = true;

            // Reset de check zodat hij de volgende keer niet weer verspringt
            BellInteraction.moetPositieHerstellen = false;
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