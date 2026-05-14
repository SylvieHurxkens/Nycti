using UnityEngine;

public class FootstepGenerator : MonoBehaviour
{
    [Header("Referenties")]
    public PlayerController playerController; 
    public AudioSource audioSource;

    [Header("Instellingen")]
    public float baseStepSpeed = 0.5f; 
    public float sprintStepSpeed = 0.3f; 
    
    [Header("Geluiden per Layer")]
    public AudioClip[] DirtClips;
    public AudioClip[] WaterClips;
    public AudioClip[] MudClips;
    public AudioClip[] StoneClips;
    public AudioClip[] WoodClips;
    public AudioClip[] defaultClips;

    private float footstepTimer = 0f;

    void Update()
    {
        // 1. Check of de speler een toets indrukt (moveInput) en op de grond staat
        // We gebruiken een kleine 'deadzone' van 0.1 om 'random' geluid bij stilstaan te voorkomen
        bool isMoving = playerController.CurrentMoveInput.magnitude > 0.1f;
        bool isGrounded = playerController.controller.isGrounded;

        if (isGrounded && isMoving)
        {
            // 2. Bepaal de snelheid van de stappen
            float currentStepInterval = (playerController.IsSprintingPublic) ? sprintStepSpeed : baseStepSpeed;

            // 3. Tel af
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                PlayFootstepSound();
                footstepTimer = currentStepInterval; // Reset naar het interval
            }
        }
        else
        {
            // 4. Als we stilstaan, zetten we de timer bijna op nul (zodat de eerste stap direct klinkt)
            // maar niet helemaal op nul, om herhaling te voorkomen.
            footstepTimer = 0.05f; 
        }
    }

    void PlayFootstepSound()
    {
        RaycastHit hit;
        // Raycast naar beneden om de layer te vinden
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);

            // Jouw switch-structuur:
            switch (layerName)
            {
                case "Dirt":
                    PlayRandomClip(DirtClips);
                    break;
                case "Water":
                    PlayRandomClip(WaterClips);
                    break;
                case "Mud":
                    PlayRandomClip(MudClips);
                    break;
                case "Stone":
                    PlayRandomClip(StoneClips);
                    break;
                case "Wood":
                    PlayRandomClip(WoodClips);
                    break;
                default:
                    PlayRandomClip(defaultClips);
                    break;
            }
        }
    }

    void PlayRandomClip(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            int index = Random.Range(0, clips.Length);
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clips[index]);
        }
    }
}