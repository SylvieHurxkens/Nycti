using UnityEngine;
using System.Collections;

public class MonsterManager : MonoBehaviour
{
    [Header("Geluid")]
    public AudioSource audioSource; 
    public AudioClip[] waarschuwingGeluiden;
    public float vertragingTijd = 3f; 

    [Header("Instellingen")]
    public GameObject monsterPrefab;
    public Transform player;
    [Range(0, 100)] public float spawnKans = 25f; 
    public float spawnInterval = 10f; 
    public float spawnDistance = 20f;

    private GameObject activeMonster;
    private bool isPlayerSafe = false;
    private float timer;

    void Start()
    {
        activeMonster = Instantiate(monsterPrefab); 
        activeMonster.SetActive(false); 
    }

    void Update()
    {
        if (player == null) return; 

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
        if (kans < spawnKans) 
        {
            StartCoroutine(MonsterAankondigingSequence());
        }
    }

    IEnumerator MonsterAankondigingSequence()
    {
        Debug.Log("Sequence is gestart! Er is een succesvolle kans gerold.");

        Vector3 spawnPos = player.position + (Random.insideUnitSphere * spawnDistance);
        RaycastHit groundHit;
        
        // We schieten nu een algemene raycast omlaag (zonder layer filter)
        if (Physics.Raycast(new Vector3(spawnPos.x, 500f, spawnPos.z), Vector3.down, out groundHit, 1000f))
        {
            // NU CHECKEN WE DE TAG: Is het object dat we raken wel echt de grond?
            if (groundHit.collider.CompareTag("Ground"))
            {
                Vector3 finalPos = groundHit.point + new Vector3(0, 0f, 0);

                if (CheckPlekVrij(finalPos)) 
                {
                    Debug.Log("De plek is VRIJ! Geluid gaat spelen.");
                    activeMonster.SetActive(false);

                    if (waarschuwingGeluiden.Length > 0)
                    {
                        int randomIndex = Random.Range(0, waarschuwingGeluiden.Length);
                        AudioClip gekozenGeluid = waarschuwingGeluiden[randomIndex];
                        AudioSource.PlayClipAtPoint(gekozenGeluid, finalPos);
                    }

                    yield return new WaitForSeconds(vertragingTijd);

                    activeMonster.transform.position = finalPos;
                    activeMonster.SetActive(true);
                    Debug.Log("Monster is NU geactiveerd op positie: " + finalPos);

                    yield return new WaitForSeconds(5f);

                    activeMonster.SetActive(false);
                    Debug.Log("Monster is weer opgeruimd.");
                }
                else
                {
                    Debug.Log("Spawn geannuleerd: CheckPlekVrij gaf FALSE terug (muur of dak geraakt).");
                }
            }
            else
            {
                Debug.Log("Spawn geannuleerd: De laser raakte iets, maar het had NIET de tag 'Ground'. Het raakte: " + groundHit.collider.name);
            }
        }
        else
        {
            Debug.Log("Spawn geannuleerd: De laser heeft helemaal niets geraakt in de leegte.");
        }
    }

    bool CheckPlekVrij(Vector3 plek)
    {
        // Dak check
        RaycastHit hit;
        if (Physics.Raycast(plek, Vector3.up, out hit, 20f))
        {
            if (hit.collider.CompareTag("Dak")) return false;
        }
        
        // Muur check
        Collider[] hitColliders = Physics.OverlapSphere(plek, 1.2f);
        foreach (var col in hitColliders)
        {
            if (col.CompareTag("Muur")) return false;
        }
        return true;
    }

    #region Safe zone logica
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("SafeZone")) isPlayerSafe = true;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("SafeZone")) isPlayerSafe = false;
    }
    #endregion
}