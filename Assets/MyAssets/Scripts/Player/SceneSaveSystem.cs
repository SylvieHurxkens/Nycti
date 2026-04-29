using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSaveSystem : MonoBehaviour
{
    private void OnEnable()
    {
        // Vertel Unity dat 'OnSceneLoaded' moet draaien zodra een scene laadt
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Netjes opruimen als het object wordt uitgeschakeld
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Deze functie wordt automatisch aangeroepen bij elk nieuw level
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Nieuw level gedetecteerd: " + scene.name);
        SaveGame(scene.name);
    }

    public void SaveGame(string sceneName)
    {
        // Slat de naam van het huidige level op
        PlayerPrefs.SetString("SavedLevel", sceneName);

        /*// Slat ook de positie op (optioneel, vaak begin je in een nieuw level op een standaardplek)
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", transform.position.z);*/

        PlayerPrefs.Save();
        Debug.Log("Spel opgeslagen bij start van " + sceneName);
    }

    // Roep deze functie aan vanuit je hoofdmenu via een "Continue" knop
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            string levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            Debug.Log("Geen savegame gevonden, start level 1.");
            SceneManager.LoadScene("Level1"); // Vul hier je eerste level in
        }
    }
}
