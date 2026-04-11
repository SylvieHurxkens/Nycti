using UnityEngine;
using System.Collections.Generic;

public class ParticleCollisionVisible : MonoBehaviour
{
    public ParticleSystem part;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[part.main.maxParticles];
        int numParticlesAlive = part.GetParticles(particles);

        // We lopen door de botsingen heen
        for (int i = 0; i < numCollisionEvents; i++)
        {
            // Voor elke botsing maken we de deeltjes op die plek zichtbaar
            // In de praktijk is het makkelijker om de deeltjes een kleur te geven
            // die 'fadet' zodra ze stilstaan.
        }
    }
}