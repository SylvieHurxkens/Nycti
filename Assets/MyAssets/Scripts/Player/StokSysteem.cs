using UnityEngine;
using UnityEngine.InputSystem; // Belangrijk!

public class StokSysteem : MonoBehaviour
{
    public ParticleSystem lichtParticles;
    public float tikBereik = 3.0f;
    public LayerMask watIsRaakbaar;

    // Deze functie wordt straks aangeroepen door je Input Action
    public void OnTik(InputAction.CallbackContext context)
    {
        // Alleen uitvoeren op het moment dat de knop echt wordt ingedrukt (performed)
        if (context.performed)
        {
            Tik();
        }
    }

    void Tik()
    {
        RaycastHit hit;
        // We schieten de straal vanuit de camera naar voren
        if (Physics.Raycast(transform.position, transform.forward, out hit, tikBereik, watIsRaakbaar))
        {
            lichtParticles.transform.position = hit.point;
            lichtParticles.transform.rotation = Quaternion.LookRotation(hit.normal);
            lichtParticles.Play();
            
            Debug.Log("Muur geraakt op: " + hit.point);
        }
    }
}
