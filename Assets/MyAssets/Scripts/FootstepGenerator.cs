using UnityEngine;

public class FootstepGenerator : MonoBehaviour
{
    [Header("Instellingen")]
    public AudioSource audioSource;
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
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check of we bewegen en op de grond staan
        if (controller.isGrounded && controller.velocity.magnitude > 0.1f)
        {
            float currentStepInterval = (Input.GetKey(KeyCode.LeftShift)) ? sprintStepSpeed : baseStepSpeed;

            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0)
            {
                PlayFootstepSound();
                footstepTimer = currentStepInterval;
            }
        }
    }

    void PlayFootstepSound()
    {
        RaycastHit hit;
        // Straal naar beneden om de layer te detecteren
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f))
        {
            int layerIndex = hit.collider.gameObject.layer;
            string layerName = LayerMask.LayerToName(layerIndex);

            // Kies de juiste lijst met geluiden op basis van de Layer naam
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

    // Handige functie om herhaling te voorkomen
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