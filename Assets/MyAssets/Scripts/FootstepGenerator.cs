using UnityEngine;

public class FootstepGenerator : MonoBehaviour
{
    [Header("Instellingen")]
    public AudioSource audioSource;
    public float baseStepSpeed = 0.5f; 
    public float sprintStepSpeed = 0.3f; 
    
    [Header("Geluiden")]
    public AudioClip[] grassClips;
    public AudioClip[] stoneClips;
    public AudioClip[] woodClips;

    private float footstepTimer = 0f;
    private CharacterController controller; 

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Controleer of de speler op de grond staat en beweegt
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            DetermineStepSpeed();
        }
    }

    void DetermineStepSpeed()
    {
        // Pas de snelheid van de voetstappen aan op basis van sprintsnelheid
        float currentStepInterval = (Input.GetKey(KeyCode.LeftShift)) ? sprintStepSpeed : baseStepSpeed;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0)
       {
           PlayFootstepSound();
            footstepTimer = currentStepInterval;
        }
    }

    void PlayFootstepSound()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            AudioClip[] selectedClips = default;

            
            switch (hit.collider.tag)
            {
                case "Grass":
                    selectedClips = grassClips;
                    break;
                case "Stone":
                    selectedClips = stoneClips;
                    break;
                case "Wood":
                    selectedClips = woodClips;
                    break;
            }

            if (selectedClips != null && selectedClips.Length > 0)
            {
                // Kies een random clip en varieer de pitch een beetje (voor realisme)
                int index = Random.Range(0, selectedClips.Length);
                audioSource.pitch = Random.Range(0.85f, 1.15f); 
                audioSource.PlayOneShot(selectedClips[index]);
            }
        }
    }
}