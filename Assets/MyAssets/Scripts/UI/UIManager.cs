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
        // Controleer bij het opstarten of er al een save bestaat
        if (continueButton != null)
        {
            // Als er geen save is, maak de knop grijs/niet klikbaar
            continueButton.interactable = PlayerPrefs.HasKey("SavedLevel");
        }

        // Zorg dat het laadscherm uit staat bij het opstarten van de UI
        if (loadingPanel != null)
        {
            loadingPanel.SetActive(false);
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("SavedLevel");

        ShowLoadingScreen();
        
        // Laden level op basis van het getal in de Build Settings
        SceneManager.LoadScene(1);
        Debug.Log("De game start vanaf level 1!");
    }
    
    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            string levelToLoad = PlayerPrefs.GetString("SavedLevel");
            
            ShowLoadingScreen();
            
            // Laad het opgeslagen level
            SceneManager.LoadScene(levelToLoad);
            Debug.Log("Game hervat in: " + levelToLoad);
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
