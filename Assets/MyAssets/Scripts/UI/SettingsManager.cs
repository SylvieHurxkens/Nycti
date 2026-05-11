using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;

    [Header("UI Elementen")]
    public Slider soundSlider;
    public Slider sensSlider;

    [Header("Instellingen")]
    public AudioMixer mainMixer;
    public static float mouseSensitivity = 1f;

    void Start()
    {
        // Laat de opgeslagen waarden
        // Als er geen save is, gebruiken we de standaardwaarden
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSens", 1f);
        float savedVolume = PlayerPrefs.GetFloat("MasterVol", 0.75f);

        // De Sliders visueel op de juiste plek zetten
        if (sensSlider != null) sensSlider.value = mouseSensitivity;
        if (soundSlider != null) soundSlider.value = savedVolume;

        // De instellingen direct toepassen zodat ze ook echt werken bij de start
        SetSensitivity(mouseSensitivity);
        SetVolume(savedVolume);
    }

    // --- NAVIGATIE ---
    
    public void OpenSettings()
    {
        pauseMenuPanel.SetActive(false); 
        settingsPanel.SetActive(true);  

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false); 
        pauseMenuPanel.SetActive(true);  
    }

    // --- LOGICA ---

    public void SetVolume(float sliderValue)
    {
        // We gebruiken Mathf.Log10 om de slider (0-1) om te zetten naar decibels (-80 tot 0)
        mainMixer.SetFloat("MasterVol", Mathf.Log10(sliderValue) * 20);

        PlayerPrefs.SetFloat("MasterVol", sliderValue);
    }

    public void SetSensitivity(float sliderValue)
    {
        mouseSensitivity = sliderValue;
        //Debug.Log("Slider waarde is nu: " + sliderValue);
        PlayerPrefs.SetFloat("MouseSens", sliderValue);
        Debug.Log("Gevoeligheid opgeslagen: " + sliderValue);
    }
}