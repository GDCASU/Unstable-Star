using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using TMPro;
using System;


public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    [Header("Levels to Load")]
    public TMPro.TextMeshPro newLevel;
    public string levelToLoad;
    [SerializeField] private GameObject noSaveFile = null;

    [Header("Select Modifiers")]
    public Button[] menuList;
    public Button[] settingsList;
    public Button[] audioList;
    public Button[] graphicsList;
    public Button[] gameplayList;
    private Button[] arrayList;
    public GameObject[] menuOptions;
    int arrayNumber;
    int type;
    bool isMenu = true;

    [Header("Audio Options")]
    [SerializeField] SoundManager soundManager;
    [SerializeField] TMP_Text masterText;
    [SerializeField] TMP_Text musicText;
    [SerializeField] TMP_Text sfxText;

    [Header("Graphics Options")]
    [SerializeField] TMP_Text brightnessText;
    [SerializeField] TMP_Text qualityText;

    [Header("Gameplay Options")]
    [SerializeField] TMP_Text sensitivityText;

    private void Start()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(this);
            return;
        }

        arrayNumber = menuList.Length;
        arrayList = menuList;
    }

    private void Update()
    {
        // scrolls through buttons
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.DownArrow)) { arrayNumber++; }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { arrayNumber--; }

        // selects and highlights buttons
        arrayList[arrayNumber % arrayList.Length].OnSelect(null);
        arrayList[arrayNumber % arrayList.Length].Select();

        // Changes the things to be selected
        ChangeMenuArray();
        
    }

    public void ChangeMenuArray() // Changes to next selection menu
    {
        int caseNumber = 0;
        for (int i=0; i< menuOptions.Length; i++)
        {
            if (menuOptions[i].activeSelf)
            {
                caseNumber = i;
            }
        }
        switch (caseNumber)
        {
            case 0:
                arrayList = menuList;
                return;
            case 1:
                arrayList = settingsList;
                return;
            case 2:
                arrayList = audioList;
                return;
            case 3:
                arrayList = graphicsList;
                return;
            case 4:
                arrayList = gameplayList;
                return;
            default:
                return;

        }
    }

    public void ChangeMenu() // changes the menu selection
    {
        isMenu = !isMenu;
    }


    // Game Controller Settings //
    public void NewGame() // loads a new game
    {
        // TODO: start new save file; refresh all unlocked levels; maybe do an "Are you sure" popup
        ScenesManager.instance.LoadNextScene(); // Loads the scene after Menu which is the first cutscene
    }

    public void LoadGame() // loads an old save file
    {
        ScenesManager.instance.LoadLevel(ScenesManager.currentLevel);
    }

    public void LoadCredits()
    {
        ScenesManager.instance.LoadScene(Scenes.Credits);
    }

    public void ExitGame() // exits application
    {
        Application.Quit();
    }

    #region Options
    // Volume Control Settings //
    public void SetMasterVolume(float volume) // changes the volume
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        soundManager.SetVolume(SoundControllers.Master, volume, 1f);

        // Changes text next to the volume to the right value
        masterText.SetText(Math.Round(volume, 1).ToString());
    }

    public void SetSFXVolume(float volume) // changes the volume
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("sfxVolume", AudioListener.volume);
        soundManager.SetVolume(SoundControllers.SFX, volume, 1f);

        // Changes text next to the volume to the right value
        sfxText.SetText(Math.Round(volume, 1).ToString());
    }

    public void SetMusicVolume(float volume) // changes the volume
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", AudioListener.volume);
        soundManager.SetVolume(SoundControllers.Music, volume, 1f);

        // Changes text next to the volume to the right value
        musicText.SetText(Math.Round(volume, 1).ToString());
    }


    // Graphics Control Settings //
    public void SetBrightness(float brightness) // changes the brightness
    {
        PlayerPrefs.SetFloat("masterBrightness", brightness);
        Screen.brightness = brightness;

        // Changes text next to the brightness to the right value
        brightnessText.SetText(Math.Round(brightness, 1).ToString());
    }

    public void SetQuality(float quality) // changes quality marked as resolution
    {
        PlayerPrefs.SetInt("masterQuality", (int)Math.Round(quality));
        QualitySettings.SetQualityLevel((int)Math.Round(quality));

        // Changes text next to the quality to the right value
        qualityText.SetText(((int)Math.Round(quality)).ToString());
    }

    public void SetFullScreen(bool fullscreen) // switches fullscreen
    {
        PlayerPrefs.SetInt("masterFullscreen", (fullscreen ? 1 : 0));
        Screen.fullScreen = fullscreen;
    }


    // Gameplay Control Settings (may be discarded) //
    public void SetSensitivity(float sensitivity) // changes sensitivity
    {
        PlayerPrefs.SetFloat("masterSen", sensitivity);
        // doesn't do anything for right now, need to figure it out based on character movement

        // Changes text next to the brightness to the right value
        sensitivityText.SetText(Math.Round(sensitivity, 1).ToString());
    }

    public void SetInversion(bool inversion) // changes inversion on Y axis
    {
        PlayerPrefs.SetInt("masterInvert", inversion ? 1 : 0);
        // doesn't do anything for right now, need to figure it out based on character movement
    }
    #endregion
}
