using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UltraForestPro : MonoBehaviour
{
    [Header("Prefabs & Doelen")]
    public GameObject[] treePrefabs; 
    public Transform player;

    [Header("Afstanden & Dichtheid")]
    public int numberOfTrees = 120;      // Verhoog dit voor een voller bos
    public float spawnRadius = 45f;      // Iets groter dan je zichtveld om 'ploppen' te verbergen
    public float safeZone = 5f;          // Ruimte om de speler heen

    [Header("Rendering")]
    public float riseSpeed = 8f;         // Hoe snel de boom uit de grond komt

    private List<Transform> treePool = new List<Transform>();

    void Start()
    {
        if (player == null) player = Camera.main.transform;

        // 1. Initialiseer de pool
        for (int i = 0; i < numberOfTrees; i++)
        {
            GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
            
            // We starten ze direct op een goede plek voor de speler
            Vector3 startPos = GetRandomPosInFront();
            GameObject t = Instantiate(prefab, startPos, GetRandomRotation());
            
            // Zet een willekeurige schaal voor variatie
            float s = Random.Range(0.8f, 1.5f);
            t.transform.localScale = new Vector3(s, s, s);

            treePool.Add(t.transform);
        }

        // 2. Start de optimalisatie loop (elke 0.1 seconde checken is genoeg)
        InvokeRepeating("UpdateForestLogic", 0.1f, 0.1f);
    }

    void UpdateForestLogic()
    {
        Vector3 playerPos = player.position;
        Vector3 playerForward = player.forward;

        foreach (Transform tree in treePool)
        {
            Vector3 offset = tree.position - playerPos;
            float distanceSqr = offset.sqrMagnitude;
            
            // Bereken of de boom achter ons staat (Dot product)
            float dot = Vector3.Dot(playerForward, offset.normalized);

            // CONDITIES VOOR VERPLAATSEN:
            // - Boom is te ver weg (> radius)
            // - OF boom staat achter ons (dot < -0.1)
            // - OF boom staat per ongeluk bovenop ons (< safeZone)
            if (distanceSqr > (spawnRadius * spawnRadius) || dot < -0.1f || distanceSqr < (safeZone * safeZone))
            {
                MoveTree(tree);
            }
        }
    }

    void MoveTree(Transform tree)
    {
        // Kies een nieuwe positie in een waaier voor de speler
        Vector3 newPos = GetRandomPosInFront();
        
        // Zet de boom tijdelijk onder de grond voor het 'rise' effect
        tree.position = new Vector3(newPos.x, -10f, newPos.z);
        tree.rotation = GetRandomRotation();

        // Start het soepele omhoog komen
        StartCoroutine(RiseTree(tree));
    }

    IEnumerator RiseTree(Transform tree)
    {
        float targetY = 0f; // Pas dit aan als je een ondergrond hebt met Raycast
        Vector3 pos = tree.position;

        while (pos.y < targetY)
        {
            pos.y += Time.deltaTime * riseSpeed;
            tree.position = pos;
            yield return null;
        }
        
        // Zorg dat hij precies op 0 eindigt
        tree.position = new Vector3(pos.x, targetY, pos.z);
    }

    Vector3 GetRandomPosInFront()
    {
        // Maak een punt in een hoek van 100 graden voor de speler
        float angle = Random.Range(-50f, 50f); 
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 direction = rotation * player.forward;

        float distance = Random.Range(safeZone + 5f, spawnRadius);
        Vector3 finalPos = player.position + direction * distance;
        
        finalPos.y = 0; // Grondhoogte
        return finalPos;
    }

    Quaternion GetRandomRotation()
    {
        return Quaternion.Euler(0, Random.Range(0, 360), 0);
    }
}


//ForestSpawner