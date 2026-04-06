using UnityEngine;
using System.Collections.Generic;

public class UltraPerformanceManager : MonoBehaviour
{
    public float detectionRadius = 25f; 
    public LayerMask objectLayer;
    [Range(1, 100)]
    public int checksPerFrame = 10; // Hoeveel objecten per frame verwerken

    private List<GameObject> allTargetObjects = new List<GameObject>();
    private List<Transform> allTransforms = new List<Transform>();
    private int currentIndex = 0;
    private float sqrRadius;

    void Awake()
    {
        sqrRadius = detectionRadius * detectionRadius;

        // We zoeken alle objecten op de specifieke laag.
        // LET OP: Doe dit alleen als de objecten aan staan bij het starten!
        GameObject[] objects = GameObject.FindObjectsOfType<GameObject>(true);
        foreach (GameObject obj in objects)
        {
            if (((1 << obj.layer) & objectLayer) != 0)
            {
                allTargetObjects.Add(obj);
                allTransforms.Add(obj.transform);
                
                // We forceren ze uit bij de start (behalve als ze heel dichtbij zijn)
                float dist = (transform.position - obj.transform.position).sqrMagnitude;
                obj.SetActive(dist < sqrRadius);
            }
        }
    }

    void Update()
    {
        int total = allTargetObjects.Count;
        if (total == 0) return;

        Vector3 playerPos = transform.position;

        // We checken slechts een klein deel van de totale lijst per frame
        for (int i = 0; i < checksPerFrame; i++)
        {
            if (currentIndex >= total) currentIndex = 0;

            GameObject target = allTargetObjects[currentIndex];
            Transform t = allTransforms[currentIndex];

            if (target != null)
            {
                float sqrDist = (playerPos - t.position).sqrMagnitude;
                bool shouldBeActive = sqrDist < sqrRadius;

                // Alleen SetActive aanroepen als de staat moet veranderen
                // Dit voorkomt onnodige rekenkracht
                if (target.activeSelf != shouldBeActive)
                {
                    target.SetActive(shouldBeActive);
                }
            }

            currentIndex++;
        }
    }
}