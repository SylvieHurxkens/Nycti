using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject loadingPanel;
    public GameObject controlsPanel;

    [Header("Buttons")]
    public Button continueButton;

    void Start()
    {
        // We controleren of de knop grijs moet zijn
        UpdateButtonStatus();

        // Zorg dat het laadscherm uit staat bij het opstarten van de UI
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }  

    private void UpdateButtonStatus()
    {
        if (continueButton != null)
        {
            // We kijken naar TWEE dingen:
            // 1. Is er een opgeslagen level? (PlayerPrefs)
            // 2. Is de game niet net beëindigd? (HeeftSaveGame uit BellInteraction)
            
            bool heeftSave = PlayerPrefs.HasKey("SavedLevel") && PlayerPrefs.GetInt("HeeftSaveGame", 0) == 1;

            if (heeftSave)
            {
                continueButton.interactable = true;  // Knop is normaal
            }
            else
            {
                continueButton.interactable = false; // Knop wordt grijs en onklikbaar
            }
        }
    }

    public void NewGame()
    {
        // Als je een nieuwe game start, zorgen we dat de continue knop daarna weer werkt
        PlayerPrefs.DeleteKey("SavedLevel");
        PlayerPrefs.SetInt("HeeftSaveGame", 1); 
        PlayerPrefs.Save();
        
        // Laden level op basis van het getal in de Build Settings
        ShowLoadingScreen();
        SceneManager.LoadScene(1);
        Debug.Log("De game start vanaf level 1!");
    }
    
    public void ContinueGame()
        {
            if (PlayerPrefs.HasKey("SavedLevel"))
            {
                string levelToLoad = PlayerPrefs.GetString("SavedLevel");
                ShowLoadingScreen();
                SceneManager.LoadScene(levelToLoad);
            }
        }

    public void OpenControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(true);
            // Optioneel: zet het hoofdmenu uit als je ze niet over elkaar wilt laten zweven
            // mainMenuPanel.SetActive(false); 
        }
    }

    public void CloseControls()
    {
        if (controlsPanel != null)
        {
            controlsPanel.SetActive(false);
            // Vergeet het hoofdmenu niet terug te zetten als je het in OpenControls hebt uitgezet
            // mainMenuPanel.SetActive(true);
        }
    }

    private void ShowLoadingScreen()
    {
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game afgesloten");
    }
}
