using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class AmbientSoundWave : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] float interval = 2.0f; // Hoe vaak de golf verschijnt
    [SerializeField] bool startOnAwake = true;

    private void Start()
    {
        if (startOnAwake)
        {
            StartCoroutine(WaveLoop());
        }
    }

    IEnumerator WaveLoop()
    {
        // Voeg een willekeurige vertraging toe bij de start
        // Zo beginnen niet alle objecten in de scene tegelijk
        yield return new WaitForSeconds(Random.Range(0f, interval));

        while (true) 
    {
        TriggerWave();
        
        // Maak het interval ook een klein beetje variabel voor een natuurlijker effect
        float randomInterval = interval + Random.Range(-0.2f, 0.2f);
        yield return new WaitForSeconds(Mathf.Max(0.1f, randomInterval));
    }
    }

    private void TriggerWave()
    {
        if (visualEffect != null)
        {
            // We sturen de positie van DIT object naar de VFX
            visualEffect.SetVector3("PlayerPosition", transform.position); 
            visualEffect.SendEvent("OnTik");
        }
    }
}