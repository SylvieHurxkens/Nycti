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
    
        // We maken een richting die 'vooruit' is, maar een beetje naar beneden (0.5 omlaag)
        Vector3 richting = (transform.forward + Vector3.down * 0.5f).normalized;

        if (Physics.Raycast(transform.position, richting, out hit, tikBereik, watIsRaakbaar))
        {
            lichtParticles.transform.position = hit.point;
            lichtParticles.transform.rotation = Quaternion.LookRotation(hit.normal);
            lichtParticles.Play();
        
            Debug.Log("Geraakt via schuine straal: " + hit.point);
        }
}
}
