using UnityEngine;
using UnityEngine.AI;

public class MonsterManager : MonoBehaviour
{
    [Header("Instellingen")]
    public GameObject monsterPrefab;
    public Transform player;
    public float spawnInterval = 10f; // Check elke 10 seconden
    public float spawnDistance = 20f;
    public LayerMask groundLayer;

    private GameObject activeMonster;
    private bool isPlayerSafe = false;
    private float timer;

    void Start()
    {
        activeMonster = Instantiate(monsterPrefab); //maakt monster aan bij de start
        activeMonster.SetActive(false); // Zet monster uit
    }

    void Update()
    {
        if (player == null) return; // Stop als er geen speler is gevonden

        // De timer loopt alleen als de speler NIET veilig is
        if (!isPlayerSafe)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                TrySpawnMonster();
                timer = 0;
            }
        }
    }

    void TrySpawnMonster()
    {
        float kans = Random.Range(0, 100);
        if (kans < 25) // 25% kans
        {
            MoveMonster();
        }
    }

    void MoveMonster()
    {
        // 1. Kies een willekeurige positie rondom de speler
        Vector3 randomDir = Random.insideUnitSphere * spawnDistance;
        Vector3 spawnPos = player.position + randomDir;
        
        RaycastHit groundHit;
        // We schieten eerst omlaag om de grond te vinden
        if (Physics.Raycast(new Vector3(spawnPos.x, 100f, spawnPos.z), Vector3.down, out groundHit, 200f, groundLayer))
        {
            Vector3 finalPos = groundHit.point + new Vector3(0, 0f, 0);

            // --- DE DAK CHECK ---
            RaycastHit ceilingHit;
            bool isBinnen = false;

            // Schiet een straal van 20 meter recht omhoog (Vector3.up)
            if (Physics.Raycast(finalPos, Vector3.up, out ceilingHit, 20f))
            {
                // Check of het object boven ons de tag "Dak" heeft
                if (ceilingHit.collider.CompareTag("Dak"))
                {
                    isBinnen = true;
                    Debug.Log("Spawn geannuleerd: Er zit een dak boven deze plek.");
                }
            }

            // --- DE MUUR CHECK (OverlapSphere) ---
            // We checken ook nog even of we niet half IN een muur staan
            Collider[] hitColliders = Physics.OverlapSphere(finalPos, 1.2f);
            bool raaktMuur = false;
            foreach (var col in hitColliders)
            {
                if (col.CompareTag("Muur"))
                {
                    raaktMuur = true;
                    break;
                }
            }

            // 2. Alleen spawnen als we NIET binnen zijn en GEEN muur raken
            if (!isBinnen && !raaktMuur)
            {
                // Verplaats het monster
                activeMonster.transform.position = finalPos;
                activeMonster.SetActive(true);
                Debug.Log("Monster succesvol buiten gespawned!");
            }
        }

        Invoke("HideMonster", 5f);
    }
    
    void HideMonster() 
    {
    activeMonster.SetActive(false);
    }

    #region Safe zone logica
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("SafeZone"))
        {
            isPlayerSafe = true;
            Debug.Log("Speler is nu VEILIG");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("SafeZone"))
        {
            isPlayerSafe = false;
            Debug.Log("Speler is nu NIET meer veilig");
        }
    }
    #endregion

}
