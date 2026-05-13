using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public RectTransform blockingRect; // Het zwarte vlak dat over de tekst ligt
    public CanvasGroup introGroup;     // Het hele paneel voor de fade-out
    
    public float revealSnelheid = 150f;
    public float wachtTijd = 3.0f;
    public float fadeSnelheid = 0.5f;

    private float startHoogte;

    void Start()
    {
        // Onthoud hoe hoog het vlak was
        startHoogte = blockingRect.sizeDelta.y;
        StartCoroutine(StartReveal());
    }

    IEnumerator StartReveal()
    {
        float huidigeHoogte = startHoogte;

        // 1. Het zwarte vlak naar beneden toe korter maken
        while (huidigeHoogte > 0)
        {
            huidigeHoogte -= revealSnelheid * Time.deltaTime;
            blockingRect.sizeDelta = new Vector2(blockingRect.sizeDelta.x, huidigeHoogte);
            yield return null;
        }

        // 2. Wacht even als de tekst helemaal vrij is
        yield return new WaitForSeconds(wachtTijd);

        // 3. De hele boel laten wegfaden
        while (introGroup.alpha > 0)
        {
            introGroup.alpha -= Time.deltaTime * fadeSnelheid;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}