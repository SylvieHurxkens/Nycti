using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BellInteraction : MonoBehaviour
{
    [Header("UI Fade")]
    public Image fadeImage; 
    public Canvas parentCanvas; 
    public float fadeDuur = 1.0f;
    public float kijkTijd = 3.0f; 

    [Header("Speler Opslag")]
    public GameObject speler; 

    public static Vector3 opgeslagenPositie;
    public static Quaternion opgeslagenRotatie;
    public static bool moetPositieHerstellen = false;

    private bool isGeactiveerd = false;

    public void OnInteract()
    {
        if (isGeactiveerd) return;
        isGeactiveerd = true;

        // 1. Sla positie op
        opgeslagenPositie = speler.transform.position;
        opgeslagenRotatie = speler.transform.rotation;
        moetPositieHerstellen = true;

        // 2. Bel animatie
        transform.DOPunchRotation(new Vector3(0, 0, 20f), 0.5f, 10, 1);

        // 3. Behoud faderscherm
        DontDestroyOnLoad(parentCanvas.gameObject);

        // Start het proces
        StartEersteOvergang();
    }

    private void StartEersteOvergang()
    {
        // We gaan naar de VOLGENDE scene (de kijk-scene)
        int kijkSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (kijkSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            fadeImage.DOFade(1f, fadeDuur).OnComplete(() =>
            {
                SceneManager.LoadScene(kijkSceneIndex);

                // Zodra geladen, fade open
                fadeImage.DOFade(0f, fadeDuur).SetDelay(0.5f).OnComplete(() =>
                {
                    // Wacht de kijktijd
                    DOVirtual.DelayedCall(kijkTijd, () => 
                    {
                        StartTweedeOvergang(kijkSceneIndex + 1);
                    });
                });
            });
        }
    }

    private void StartTweedeOvergang(int finaleIndex)
    {
        // Fade weer naar zwart
        fadeImage.DOFade(1f, fadeDuur).OnComplete(() =>
        {
            // Controleer of de finale scene bestaat
            if (finaleIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(finaleIndex);
                
                // Laatste fade-in
                fadeImage.DOFade(0f, fadeDuur).SetDelay(0.5f).OnComplete(() =>
                {
                    Destroy(parentCanvas.gameObject);
                });
            }
            else
            {
                // Als er GEEN volgende scene is (einde van de game of terug naar 0)
                Debug.Log("Geen finale scene gevonden, terug naar index 0");
                
                // MAAK DE MUIS WEER ZICHTBAAR
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                SceneManager.LoadScene(0);
                fadeImage.DOFade(0f, fadeDuur).OnComplete(() => {
                    Destroy(parentCanvas.gameObject);
                });
            }
        });
    }
}