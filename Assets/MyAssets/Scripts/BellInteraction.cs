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

    // Deze blijven static voor de huidige sessie
    public static Vector3 opgeslagenPositie;
    public static Quaternion opgeslagenRotatie;
    public static bool moetPositieHerstellen = false;

    private bool isGeactiveerd = false;

    public void OnInteract()
    {
        if (isGeactiveerd) return;
        isGeactiveerd = true;

        // 1. Sla positie op voor de huidige sessie
        opgeslagenPositie = speler.transform.position;
        opgeslagenRotatie = speler.transform.rotation;
        moetPositieHerstellen = true;

        // 2. Sla permanent op dat we kunnen 'Continueren' (voor het menu)
        PlayerPrefs.SetInt("HeeftSaveGame", 1);
        PlayerPrefs.Save();

        // 3. Bel animatie
        transform.DOPunchRotation(new Vector3(0, 0, 20f), 0.5f, 10, 1);

        // 4. Behoud faderscherm
        DontDestroyOnLoad(parentCanvas.gameObject);

        // Start het proces
        StartEersteOvergang();
    }

    private void StartEersteOvergang()
    {
        int kijkSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (kijkSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            fadeImage.DOFade(1f, fadeDuur).OnComplete(() =>
            {
                SceneManager.LoadScene(kijkSceneIndex);

                fadeImage.DOFade(0f, fadeDuur).SetDelay(0.5f).OnComplete(() =>
                {
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
        fadeImage.DOFade(1f, fadeDuur).OnComplete(() =>
        {
            if (finaleIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(finaleIndex);
                
                fadeImage.DOFade(0f, fadeDuur).SetDelay(0.5f).OnComplete(() =>
                {
                    Destroy(parentCanvas.gameObject);
                });
            }
            else
            {
                Debug.Log("Geen finale scene gevonden, game is uitgespeeld.");
                
                // Wis de save-status zodat de continu-knop verdwijnt
                PlayerPrefs.SetInt("HeeftSaveGame", 0);
                PlayerPrefs.Save();
                moetPositieHerstellen = false;

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