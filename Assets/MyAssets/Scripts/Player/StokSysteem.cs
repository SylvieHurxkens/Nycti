using UnityEngine;
using UnityEngine.InputSystem; 

public class StokSysteem : MonoBehaviour
{
public ParticleSystem lichtParticles;
public Transform player;
public SoundWave soundWave;
//public int aantalParticles = 200;

    // Deze functie wordt straks aangeroepen door je Input Action
    public void OnTik(InputValue context)
    {
        // Alleen uitvoeren op het moment dat de knop echt wordt ingedrukt (performed)

            Tik();
        
    }

   void Tik()
    {
        //    lichtParticles.Emit(aantalParticles);
            soundWave.TriggerWave(player.position);
            Debug.Log("blurp ");
      
    }
}
