using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProximityManagerOptimized : MonoBehaviour
{
    public float detectionRadius = 10f;
    public LayerMask objectLayer;
    public float checkInterval = 0.2f; // Hoe vaak checken? (0.2 = 5 keer per seconde)

    private List<MeshRenderer> activeRenderers = new List<MeshRenderer>();

    void Start()
    {
        // 1. Zoek bij de start EENMALIG alle relevante objecten en zet ze uit
        // Dit voorkomt lag tijdens het lopen
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (((1 << obj.layer) & objectLayer) != 0)
            {
                MeshRenderer rend = obj.GetComponent<MeshRenderer>();
                if (rend != null) rend.enabled = false;
            }
        }

        // 2. Start de herhalende check
        StartCoroutine(ProximityCheckRoutine());
    }

    IEnumerator ProximityCheckRoutine()
    {
        while (true)
        {
            PerformCheck();
            // Wacht even voordat we de volgende check doen (bespaart CPU)
            yield return new WaitForSeconds(checkInterval);
        }
    }

    void PerformCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, objectLayer);
        List<MeshRenderer> currentlyInView = new List<MeshRenderer>();

        // Zet nieuwe objecten aan
        foreach (var hit in hitColliders)
        {
            MeshRenderer rend = hit.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                currentlyInView.Add(rend);
                if (!rend.enabled) rend.enabled = true;
            }
        }

        // Zet oude objecten uit
        for (int i = activeRenderers.Count - 1; i >= 0; i--)
        {
            MeshRenderer oldRend = activeRenderers[i];
            if (oldRend != null && !currentlyInView.Contains(oldRend))
            {
                oldRend.enabled = false;
                activeRenderers.RemoveAt(i);
            }
        }

        activeRenderers = currentlyInView;
    }
}