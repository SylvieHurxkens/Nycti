using UnityEngine;

public class StokTik : MonoBehaviour
{
    public ParticleSystem lichtParticles;
    public float bereik = 2.0f;
    public LayerMask muurLayer; // Selecteer hier de layers van je muren/vloer

    void Update()
    {
        // Linkermuisknop of een andere actieknop
        if (Input.GetMouseButtonDown(0))
        {
            Tik();
        }
    }

    void Tik()
    {
        RaycastHit hit;
        // Schiet een straal naar voren vanuit de stok/camera
        if (Physics.Raycast(transform.position, transform.forward, out hit, bereik, muurLayer))
        {
            // Verplaats het systeem naar het punt van de inslag
            lichtParticles.transform.position = hit.point;
            
            // Richt de particles weg van de muur op basis van de 'Normal'
            lichtParticles.transform.rotation = Quaternion.LookRotation(hit.normal);
            
            // Vuurt de burst af
            lichtParticles.Play();

            // Optioneel: speel geluid af
            // AudioSource.PlayClipAtPoint(tikGeluid, hit.point);
        }

        /*// Voeg dit toe aan je Raycast logica
        if (hit.collider.CompareTag("Monster")) 
        {
            // Laat de deeltjes rood kleuren als ze een onzichtbaar monster raken
            var main = lichtParticles.main;
            main.startColor = Color.red;
    
            // Of laat het monster een geluid maken als je hem raakt met je stok
        hit.collider.GetComponent<MonsterScript>().SchrikReactie();
        }
        else 
        {
            var main = lichtParticles.main;
            main.startColor = Color.white;
        }*/
    }    
        
}