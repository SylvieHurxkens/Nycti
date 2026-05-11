using UnityEngine;

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
    // Kiest een willekeurige plek rondom de speler op de X en Z as
    Vector3 randomDir = Random.insideUnitSphere * spawnDistance;
    Vector3 checkPos = player.position + randomDir;
    checkPos.y = 100f; // Zet de startpositie van de "laser" hoog in de lucht

    RaycastHit hit; // Schiet een Raycast

    if (Physics.Raycast(checkPos, Vector3.down, out hit, 200f, groundLayer))
        {
            // Verplaats het bestaande monster naar de nieuwe plek
            activeMonster.transform.position = hit.point + new Vector3(0, 0, 0);
             
            activeMonster.SetActive(true);

            Debug.Log("Het monster is verplaatst naar de buurt van de speler!");
        }
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
