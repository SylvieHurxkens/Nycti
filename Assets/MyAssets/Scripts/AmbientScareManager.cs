using UnityEngine;
using System.Collections.Generic;

public class AmbientScareManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] scaryClips;

    public float minTimeBetweenScares = 15f;
    public float maxTimeBetweenScares = 45f;

    private float timer;

    void Start()
    {
        // Zet de eerste timer op een willekeurige tijd
        ResetTimer();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            PlayScarySound();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        timer = Random.Range(minTimeBetweenScares, maxTimeBetweenScares);
    }

    void PlayScarySound()
    {
        if (scaryClips.Length > 0 && !audioSource.isPlaying)
        {
            int index = Random.Range(0, scaryClips.Length);
            
            // Subtiele volume- en pitchvariatie voor extra onvoorspelbaarheid
            audioSource.pitch = Random.Range(0.85f, 1.1f);
            audioSource.volume = Random.Range(0.7f, 1.0f);
            
            audioSource.PlayOneShot(scaryClips[index]);
        }
    }
}