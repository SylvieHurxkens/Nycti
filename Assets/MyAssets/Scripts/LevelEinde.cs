using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEinde : MonoBehaviour
{
    // Controleert of het de speler is 
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            LadenVolgendeLevel();
        }
    }

    public void LadenVolgendeLevel()
    {
        // Haal het nummer op van de huidige scene en tel er 1 bij op
        int volgendeSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Controleer of er een volgende scene is
        if (volgendeSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(volgendeSceneIndex);
        }
        else
        {
            Debug.Log("Gefeliciteerd! Je hebt alle levels uitgespeeld.");
            // Optioneel: Ga terug naar het hoofdmenu
            // SceneManager.LoadScene(0); 
        }
    }
}
