/*
 * Note from Aaron
 *      I had to stop 
 */

using System;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

enum CurrentMenu
{
    ObjectsSelection,
    PrimaryOptions,
    Settings,
    Audio,
    Gameplay,
    Graphics
}

public class MenuManager : MonoBehaviour
{
    [Header("Menu Look Effect")]
    [SerializeField] [Range(0f,10f)] private float lookAroundFreedom;

    [Header("Object Selection References")]
    [SerializeField] MenuOption[] menuOptions;  // Credits paper stack and menu chair
    int menuOptionIndex = 0;                    // Holds current selected index of menuOptions

    [Header("Screen Navigation References")]
    [SerializeField] GameObject primaryOptionsContainer;
    [SerializeField] Selectable[] primaryOptions;
    [SerializeField] GameObject settingsOptionsContainer;
    [SerializeField] Selectable[] settingsOptions;
    [SerializeField] GameObject audioOptionsContainer;
    [SerializeField] Selectable[] audioOptions;
    [SerializeField] GameObject gameplayOptionsContainer;
    [SerializeField] Selectable[] gameplayOptions;
    [SerializeField] GameObject graphicsOptionsContainer;
    [SerializeField] Selectable[] graphicsOptions;

    // Stores the number of options in 
    Dictionary<CurrentMenu, int> numOptions = new Dictionary<CurrentMenu, int>()
    {
        {CurrentMenu.ObjectsSelection, 2 },
        {CurrentMenu.PrimaryOptions, 3 },
        {CurrentMenu.Settings, 4 },
        {CurrentMenu.Audio, 4 },
        {CurrentMenu.Gameplay, 4 },
        {CurrentMenu.Graphics, 3 }
    };

    CurrentMenu currentMenu = CurrentMenu.ObjectsSelection; // Current menu
    int optionsIndex = 0;   // Current menu selection.

    [Header("Console Objects")]
    [SerializeField] private TMP_Text consoleActText;
    [SerializeField] private levelButton upConsoleButton;
    [SerializeField] private levelButton downConsoleButton;

    [Header("Audio Options")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TMP_Text masterSliderValue;
    [SerializeField] private TMP_Text musicSliderValue;
    [SerializeField] private TMP_Text sfxSliderValue;
    
    [Header("Graphics Options")]
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider brightnessSlider;
    [SerializeField] private Slider qualitySlider;
    [SerializeField] TMP_Text brightnessValue;
    [SerializeField] TMP_Text qualityValue;

    [Header("Gameplay Options")]
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] TMP_Text sensitivityValue;
    [SerializeField] private Toggle invertYToggle;

    [Header("Cheats")]
    [SerializeField] private GameObject CheatMenuOption;

    // Local variables
    private List<GameAct> ActList = new List<GameAct>();
    private int currentActIndex;
    private Vector3 defaultCameraRotation;
    private Vector3 defaultCameraPosition;

    // Class to help load acts
    private class GameAct
    {
        public string actName;
        public bool isUnlocked;
        public bool isCompleted;
        public Scenes sceneEnumValue;
        public Color textColor;

        public GameAct(string actName, bool isUnlocked, bool isCompleted, Scenes sceneEnumValue)
        {
            this.actName = actName;
            this.isUnlocked = isUnlocked;
            this.isCompleted = isCompleted;
            this.sceneEnumValue = sceneEnumValue;

            // Color logic for console window
            if (isCompleted)
            {
                // Level is complete, make text green
                textColor = Color.green;
            }
            else if (isUnlocked)
            {
                // Level was not complete but unlocked, make text yellow
                textColor = Color.yellow;
            }
            else
            {
                // Level was locked, make text red
                textColor = Color.red;
            }
        }
    }

    private void Start()
    {
        // Get camera rotation and position
        defaultCameraRotation = Camera.main.transform.eulerAngles;
        defaultCameraPosition = Camera.main.transform.position;

        // Populate List of acts

        // IAN HACK: I should really be serializing this into the save instead of by bits
        // but this is faster for now. On the other side, enums dont serialize well, and the scenes enum would
        // be a problem

        // ACT 1
        bool isAct1Completed = SerializedDataManager.instance.gameData.isAct1Complete;
        GameAct act1 = new GameAct("ACT 1", true, isAct1Completed, Scenes.Weapon_Select_1);

        // ACT 2
        bool isAct2Unlocked = SerializedDataManager.instance.gameData.isAct2Unlocked;
        bool isAct2Completed = SerializedDataManager.instance.gameData.isAct2Complete;
        GameAct act2 = new GameAct("ACT 2", isAct2Unlocked, isAct2Completed, Scenes.Weapon_Select_2);

        // ACT 3
        bool isAct3Unlocked = SerializedDataManager.instance.gameData.isAct3Unlocked;
        bool isAct3Completed = SerializedDataManager.instance.gameData.isAct3Complete;
        GameAct act3 = new GameAct("ACT 3", isAct3Unlocked, isAct3Completed, Scenes.Weapon_Select_3);

        // Add them to the list
        ActList.Add(act1);
        ActList.Add(act2);
        ActList.Add(act3);

        // Set the console text to the first act and set the color
        consoleActText.text = act1.actName;
        consoleActText.color = act1.textColor;
        currentActIndex = 0;

        // Set the slider and toggle values from the ones loaded from data

        // Audio
        masterSlider.value = SerializedDataManager.instance.configData.masterVolumeValue;
        sfxSlider.value = SerializedDataManager.instance.configData.sfxVolumeValue;
        musicSlider.value = SerializedDataManager.instance.configData.musicVolumeValue;

        // Gameplay
        sensitivitySlider.value = SerializedDataManager.instance.configData.sensitivity;
        invertYToggle.isOn = SerializedDataManager.instance.configData.invertYAxis;

        // Graphics
        fullscreenToggle.isOn = SerializedDataManager.instance.configData.fullscreen;
        brightnessSlider.value = SerializedDataManager.instance.configData.brightness;
        qualitySlider.value = SerializedDataManager.instance.configData.quality;

        // Cheats
        if (GameSettings.instance.areCheatsUnlocked)
        {
            CheatMenuOption.SetActive(true);
        }
        else
        {
            CheatMenuOption.SetActive(false);
        }

        // Subscribe buttons to switch events
        upConsoleButton.ButtonPressed += NextActConsoleOption;
        downConsoleButton.ButtonPressed += PreviousActConsoleOption;
    }

    private void Update()
    {
        // IAN: Update camera position for the look around effect
        if (Camera.main.transform.position == defaultCameraPosition)
        {
            Camera.main.transform.eulerAngles = GetTargetRotation();
        }

        // IAN HACK: This code is also here so the cheats can be rendered on the main menu without having to
        // reload the scene
        if (GameSettings.instance.areCheatsUnlocked)
        {
            CheatMenuOption.SetActive(true);
        }
        else
        {
            CheatMenuOption.SetActive(false);
        }
    }

    #region Main Menu Navigation

    /// <summary>
    /// Handles the navigation of the main menu while still in the Object
    /// Selection phase.
    /// </summary>
    /// <param name="dir">Direction to navigate towards.</param>
    void NavigateObjectsSelection(Vector2 dir)
    {
        // Swap selection to the next object selection. (since there's only 2 options this can be easy or proper)
        // Ensure that the player knows where they're navigating to.
    }

    /// <summary>
    /// Handles the navigation of the main menu while not in the Object
    /// Selection phase.
    /// </summary>
    /// <param name="dir">Direction to navigate towards.</param>
    void NavigateOptions(Vector2 dir)
    {
        // Navigate up
        if (dir.y > 0)
        {
            // Increment by 1, then modulo by numOptions plus number of act selectors
            optionsIndex = (optionsIndex + 1) % (numOptions[currentMenu] + 2);
        }
        // Navigate down
        else if (dir.y < 0)
        {
            optionsIndex--; // Decrement index

            // Set to highest (add act selectors!!)
            if (optionsIndex < 0) optionsIndex = numOptions[currentMenu] + 1;
        }

        // Select new option
        if(optionsIndex < numOptions[currentMenu])
        {

        }
    }

    /// <summary>
    /// Utility function that just ensures that the proper UI
    /// option is highlighted.
    /// 
    /// optionsIndex MUST be in range [0, numOptions[currentMenu] + 1].
    /// </summary>
    void SelectOption()
    {
        // Regular option
        if (optionsIndex < numOptions[currentMenu])
        {
            switch (currentMenu)
            {
                case CurrentMenu.ObjectsSelection:
                    // TODO: Make menu option glow green
                    break;
                case CurrentMenu.PrimaryOptions:
                    primaryOptions[optionsIndex].Select();
                    break;
                case CurrentMenu.Settings:

                    break;
                case CurrentMenu.Audio:

                    break;
                case CurrentMenu.Gameplay:

                    break;
                case CurrentMenu.Graphics:

                    break;
            }
        }
        // Act selection
        else
        {
            int whichActArrow = (optionsIndex - numOptions[currentMenu]) % 2;     // will be either 0 or 1.

            // Up arrow
            if(whichActArrow == 0)
            {
                // Make up arrow glow green
            }
            // Down arrow
            else
            {
                // Make down arrow glow green
            }
        }
    }

    #endregion

    #region Scene navigation

    public void ResetProgress() // loads a new game
    {
        // Reset file progress
        SerializedDataManager.instance.NewGame();
        // Go back to the loading screen
        ScenesManager.instance.LoadScene(Scenes.IntroLoadingScreen);
    }

    public void StartAct() // Loads the scene of the selected act in the console
    {
        // Only load the act if unlocked, made it " == false " to make it more readable
        if (ActList[currentActIndex].isUnlocked == false)
        {
            // TODO: Make an error beep sound or something
            return;
        }
        // Else, load the scene
        Scenes selectedScene = ActList[currentActIndex].sceneEnumValue;
        ScenesManager.instance.LoadScene(selectedScene);
    }

    public void LoadCredits()
    {
        ScenesManager.instance.LoadScene(Scenes.Credits);
    }

    public void ExitGame() // exits application
    {
        Application.Quit();
    }

    // Function needed to use UnlockAll Properly
    public void ReloadMainMenu()
    {
        ScenesManager.instance.LoadScene(Scenes.MainMenu);
    }

    #endregion

    #region Console Act Text

    /// <summary>
    /// Function that will be triggered by the console buttons, cycling the act selection foward
    /// </summary>
    public void NextActConsoleOption()
    {
        currentActIndex++;
        // Check for array out of bounds
        if (ActList.Count - 1 < currentActIndex)
        {
            currentActIndex = 0;
            SetConsoleText(ActList[currentActIndex].actName, ActList[currentActIndex].textColor);
            return;
        }
        SetConsoleText(ActList[currentActIndex].actName, ActList[currentActIndex].textColor);
    }

    /// <summary>
    /// Function that will be triggered by the console buttons, cycling the act selection backwards
    /// </summary>
    public void PreviousActConsoleOption()
    {
        currentActIndex--;
        // Check for array out of bounds
        if (currentActIndex < 0)
        {
            currentActIndex = ActList.Count - 1;
            SetConsoleText(ActList[currentActIndex].actName, ActList[currentActIndex].textColor);
            return;
        }
        SetConsoleText(ActList[currentActIndex].actName, ActList[currentActIndex].textColor);
    }

    /// <summary>
    /// Helper function to set the text of the console act display
    /// </summary>
    public void SetConsoleText(string text, Color color)
    {
        consoleActText.text = text;
        consoleActText.color = color;
    }

    #endregion

    #region Audio Options

    // Volume Control Settings //

    public void SetMasterVolume(float volume)
    {
        int integerVolume = (int)volume;
        SoundManager.instance.SetVolume(SoundControllers.Master, integerVolume, 100f);
        // Changes text next to the volume to the right value
        masterSliderValue.SetText(integerVolume.ToString());
    }

    public void SetSFXVolume(float volume)
    {
        int integerVolume = (int)volume;
        SoundManager.instance.SetVolume(SoundControllers.SFX, integerVolume, 100f);
        // Changes text next to the volume to the right value
        sfxSliderValue.SetText(Mathf.Round(volume).ToString());
    }

    public void SetMusicVolume(float volume)
    {
        int integerVolume = (int)volume;
        SoundManager.instance.SetVolume(SoundControllers.Music, integerVolume, 100f);
        // Changes text next to the volume to the right value
        musicSliderValue.SetText(integerVolume.ToString());
    }

    #endregion

    #region Graphic Options

    // Graphics Control Settings //
    public void SetBrightness(float brightness) // changes the brightness
    {
        int integerBrightness = (int)brightness;
        SerializedDataManager.instance.configData.brightness = integerBrightness;
        Screen.brightness = integerBrightness;

        // Changes text next to the brightness to the right value
        brightnessValue.SetText(integerBrightness.ToString());
    }

    public void SetQuality(float quality) // changes quality marked as resolution
    {
        int integerQuality = (int)quality;
        SerializedDataManager.instance.configData.quality = integerQuality;
        QualitySettings.SetQualityLevel(integerQuality);

        // Changes text next to the quality to the right value
        qualityValue.SetText(integerQuality.ToString());
    }

    public void SetFullScreen(bool fullscreen) // switches fullscreen
    {
        SerializedDataManager.instance.configData.fullscreen = fullscreen;
        Screen.fullScreen = fullscreen;
    }

    #endregion

    #region Gameplay Options

    // Gameplay Control Settings (may be discarded) //
    public void SetSensitivity(float sensitivity) // changes sensitivity
    {
        int integerSensitivity = (int)sensitivity;
        SerializedDataManager.instance.configData.sensitivity = integerSensitivity;
        
        // doesn't do anything for right now, need to figure it out based on character movement

        // Changes text next to the brightness to the right value
        sensitivityValue.SetText(integerSensitivity.ToString());
    }

    public void SetInversion(bool inversion) // changes inversion on Y axis
    {
        SerializedDataManager.instance.configData.invertYAxis = inversion;
        // doesn't do anything for right now, need to figure it out based on character movement
    }

    #endregion

    #region CHEATS

    /// <summary>
    /// Function that will toggle the damage of the pistol from 3 to 99
    /// </summary>
    public void ToggleLethalPistol(bool toggle)
    {
        GameSettings.instance.isPistolLethal = toggle;
    }

    /// <summary>
    /// Function that will set the health of the player to 100
    /// </summary>
    public void Toggle100HealthMode(bool toggle)
    {
        GameSettings.instance.isHealth100 = toggle;
    }

    /// <summary>
    /// Option that will unlock all
    /// </summary>
    public void unlockAll()
    {
        // Ian: Unlocks all, this assumes cheats are already unlocked, which means everything is unlocked, so idk
        // how to use this besides playtesting xd
        SerializedDataManager.instance.gameData.isAct1Complete = true;
        SerializedDataManager.instance.gameData.isAct2Unlocked = true;
        SerializedDataManager.instance.gameData.isAct2Complete = true;
        SerializedDataManager.instance.gameData.isAct3Unlocked = true;
        SerializedDataManager.instance.gameData.isAct3Complete = true;
        SerializedDataManager.instance.gameData.isGatlingUnlocked = true;
        SerializedDataManager.instance.gameData.isLaserUnlocked = true;
        SerializedDataManager.instance.gameData.isProxiBombUnlocked = true;
        SerializedDataManager.instance.gameData.isPhaseShiftUnlocked = true;
    }

    #endregion

    // Function used to make the camera move with the look around effect
    public Vector3 GetTargetRotation()
    {
        // Get the mouse position
        float maxX = Screen.width;
        float midX = maxX / 2;
        float mouseX = Mathf.Max(Mathf.Min(Input.mousePosition.x, maxX), 0);
        float rotation = lookAroundFreedom * Mathf.Sign(mouseX - midX) * Mathf.Pow(Mathf.Abs(((mouseX - midX) / midX)), 2);
        return defaultCameraRotation + new Vector3(0f, rotation, 0f);
    }
}