using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject loadingPanel;

    [Header("Buttons")]
    public Button continueButton;

    void Start()
    {
        // Controleer bij het opstarten van de UI of er een save bestaat
        if (continueButton != null)
        {
            if (!PlayerPrefs.HasKey("SavedLevel"))
            {
                continueButton.interactable = false;
            }
            else
            {
                continueButton.interactable = true;
            }
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("SavedLevel");
        // Laden level op basis van het getal in de Build Settings
        SceneManager.LoadScene(1);
        Debug.Log("De game start vanaf level 1!");
    }
    
    

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game afgesloten");
    }
}
