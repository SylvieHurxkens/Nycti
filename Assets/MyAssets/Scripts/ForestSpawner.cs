using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowForest : MonoBehaviour
{
    public GameObject[] treePrefabs;
    public Transform player;
    
    [Header("Instellingen")]
    public int numberOfTrees = 100;
    public float radius = 30f;
    public float safeZone = 6f; // Ruimte direct om de speler heen
    
    private List<Transform> treePool = new List<Transform>();

   void Start()
{
    if (player == null) player = Camera.main.transform;

    for (int i = 0; i < numberOfTrees; i++)
    {
        GameObject prefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
        // INITIALISATIE: We zetten ze nu direct op een geldige plek voor je
        GameObject t = Instantiate(prefab, GetRandomPosInFront(), Quaternion.Euler(0, Random.Range(0, 360), 0));
        treePool.Add(t.transform);
    }

    InvokeRepeating("UpdateForest", 0.1f, 0.1f);
}

void UpdateForest()
{
    Vector3 playerPos = player.position;
    Vector3 playerForward = player.forward;

    foreach (Transform tree in treePool)
    {
        Vector3 offset = tree.position - playerPos;
        float distanceSqr = offset.sqrMagnitude;

        // BEREKENING: Staat de boom achter ons of te dichtbij?
        float dot = Vector3.Dot(playerForward, offset.normalized);

        // CONDITIE: 
        // 1. Te ver weg (radius)
        // 2. Te dichtbij (safeZone)
        // 3. Achter de speler (dot < 0)
        if (distanceSqr > (radius * radius) || distanceSqr < (safeZone * safeZone) || dot < 0.1f) 
        {
            tree.position = GetRandomPosInFront();
            tree.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }
}

    Vector3 GetRandomPosInFront()
    {
        // Genereer een punt in een kegel/waaier voor de speler
        float angle = Random.Range(-60f, 60f); // 120 graden gezichtsveld
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 direction = rot * player.forward;

        float dist = Random.Range(safeZone, radius);
        Vector3 spawnPos = player.position + direction * dist;
        
        spawnPos.y = 0; // Of gebruik je Raycast hier voor hoogte
        return spawnPos;
    }
}

//ForestSpawner