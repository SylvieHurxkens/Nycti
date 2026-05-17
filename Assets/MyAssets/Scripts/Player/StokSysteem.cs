using UnityEngine;
using UnityEngine.InputSystem; 

public class StokSysteem : MonoBehaviour
{
    [Header("Referenties")]
    public ParticleSystem lichtParticles;
    public Transform player;
    public SoundWave soundWave;
    //public int aantalParticles = 200;
    public AudioSource audioSource; // De AudioSource voor de stok

    [Header("Stok Tik Geluiden")]
    public AudioClip[] DirtTikClips;
    public AudioClip[] StoneTikClips;
    public AudioClip[] WoodTikClips;
    public AudioClip[] defaultTikClips;

    // Deze functie wordt straks aangeroepen door je Input Action
    public void OnTik(InputValue context)
    {
        // Alleen uitvoeren op het moment dat de knop echt wordt ingedrukt (performed)
        Tik();
        
    }

   void Tik()
    {
        // lichtParticles.Emit(aantalParticles);
        soundWave.TriggerWave(player.position);
        Debug.Log("blurp ");
      
        PlayTikSound();
    }

    void PlayTikSound()
    {
        RaycastHit hit;
        // We schieten een straal vanaf de speler naar beneden om de grond te checken
        if (Physics.Raycast(player.position, Vector3.down, out hit, 2.0f))
        {
            string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
           
            switch (layerName)
            {
                case "Dirt":
                    PlayRandomTik(DirtTikClips);
                    break;
                case "Stone":
                    PlayRandomTik(StoneTikClips);
                    break;
                case "Wood":
                    PlayRandomTik(WoodTikClips);
                    break;
                default:
                    PlayRandomTik(defaultTikClips);
                    break;
            }
        }
    }

    void PlayRandomTik(AudioClip[] clips)
    {
        if (clips != null && clips.Length > 0)
        {
            int index = Random.Range(0, clips.Length);
            
            // Subtiele variatie in toonhoogte voor realisme
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(clips[index]);
        }
    }
}

