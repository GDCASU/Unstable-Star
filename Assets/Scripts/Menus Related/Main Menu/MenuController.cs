using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum CurrentMenu
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
    [Header("Miscelaneous")]
    [SerializeField] [Range(0f,10f)] private float lookAroundFreedom;
    [SerializeField] float lookAroundSensitivity = 1;       // How quickly the camera will move around due to the mouse.
    Vector3 targetRotation;
    Coroutine crtAngleCamera;

    [SerializeField] float sliderIncrements = 10;           // Number of "increments" sliders are broken up into. How many times a user has to click in order for a slider to go from min to max.

    [Header("Screen Navigation References")]
    [SerializeField] MenuOption[] menuOptions;  // Credits paper stack and menu chair
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
        {CurrentMenu.Graphics, 4 }
    };

    CurrentMenu currentMenu = CurrentMenu.ObjectsSelection; // Current menu
    int optionsIndex = 0;   // Current menu selection.

    [Header("Console Objects")]
    [SerializeField] private TMP_Text consoleActText;
    [SerializeField] private levelButton upConsoleButton;
    [SerializeField] private levelButton downConsoleButton;
    [SerializeField] Light upConsoleBtnLight;
    [SerializeField] Light downConsoleBtnLight;
    [SerializeField] Color highlightedLightColor = new Color(0, 1, 0, 1);
    Color originalLightColor;

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

    [Header("Debugging")]
    [SerializeField] bool printDebugs = false;

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

    #region Unity Events

    private void Start()
    {
        // Get camera rotation and position
        defaultCameraRotation = Camera.main.transform.eulerAngles;
        targetRotation = defaultCameraRotation;
        defaultCameraPosition = Camera.main.transform.position;

        // Populate List of acts

        // IAN HACK: I should really be serializing this into the save instead of by bits
        // but this is faster for now. On the other side, enums dont serialize well, and the scenes enum would
        // be a problem

        // ACT 1
        bool isAct1Completed = SerializedDataManager.instance.gameData.isAct1Complete;
        GameAct act1 = new GameAct("ACT 1", true, isAct1Completed, Scenes.CutScene_1);

        // ACT 2
        bool isAct2Unlocked = SerializedDataManager.instance.gameData.isAct2Unlocked;
        bool isAct2Completed = SerializedDataManager.instance.gameData.isAct2Complete;
        GameAct act2 = new GameAct("ACT 2", isAct2Unlocked, isAct2Completed, Scenes.CutScene_2);

        // ACT 3
        bool isAct3Unlocked = SerializedDataManager.instance.gameData.isAct3Unlocked;
        bool isAct3Completed = SerializedDataManager.instance.gameData.isAct3Complete;
        GameAct act3 = new GameAct("ACT 3", isAct3Unlocked, isAct3Completed, Scenes.CutScene_4);

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

        // Record original light color
        originalLightColor = upConsoleBtnLight.color;

        // Subscribe to input system.
        PlayerInput.OnMenuNavigate += NavigateOptions;
        PlayerInput.OnMousePoint += SetTargetRotation;
        PlayerInput.OnSubmit += SelectItem;
        PlayerInput.OnCancel += LoadPreviousMenu;
        PlayerInput.instance.ActivateUiControls();

        HighlightSelection();   // First selection should be highlighted.

        crtAngleCamera = StartCoroutine(AngleTowardsTargetRotation());   // Enable camera movement
    }

    private void Update()
    {
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

    private void OnDestroy()
    {
        // Clean up events and reset input system to default.
        PlayerInput.OnMenuNavigate -= NavigateOptions;
        PlayerInput.OnMousePoint -= SetTargetRotation;
        PlayerInput.OnSubmit -= SelectItem;
        PlayerInput.OnCancel -= LoadPreviousMenu;
        PlayerInput.instance.ActivateShipControls();
    }

    #endregion

    #region Main Menu Navigation

    /// <summary>
    /// Navigates backwards through the pages.
    /// </summary>
    public void LoadPreviousMenu()
    {
        if (printDebugs) Debug.Log("MenuController::GoBack");

        UnhighlightSelection(); // Almost every back scenario highlights a new item.

        switch (currentMenu)
        {
            case CurrentMenu.ObjectsSelection:
                // Can't go back from objects selection
                break;
            case CurrentMenu.PrimaryOptions:    // Load Objects Selection
                menuOptions[0].ResetCamera();

                currentMenu = CurrentMenu.ObjectsSelection;
                break;
            case CurrentMenu.Settings:  // Load primary
                settingsOptionsContainer.SetActive(false);
                primaryOptionsContainer.SetActive(true);
                
                currentMenu = CurrentMenu.PrimaryOptions;
                break;
            case CurrentMenu.Audio:     // Load settings
                audioOptionsContainer.SetActive(false);
                settingsOptionsContainer.SetActive(true);

                currentMenu= CurrentMenu.Settings;
                break;
            case CurrentMenu.Gameplay:  // Load settings
                gameplayOptionsContainer.SetActive(false);
                settingsOptionsContainer.SetActive(true);

                currentMenu = CurrentMenu.Settings;
                break;
            case CurrentMenu.Graphics:  // Load settings
                graphicsOptionsContainer.SetActive(false);
                settingsOptionsContainer.SetActive(true);

                currentMenu = CurrentMenu.Settings;
                break;
        }
        optionsIndex = 0;

        HighlightSelection();   // Ensure newly selected item is highlighted.
    }

    /// <summary>
    /// Handles the navigation of the main menu while not in the Object
    /// Selection phase.
    /// </summary>
    /// <param name="dir">Direction to navigate towards.</param>
    void NavigateOptions(Vector2 dir)
    {
        if (printDebugs) Debug.Log("MenuController::NavigateOptions");
        if (dir == Vector2.zero) return;    // No input.

        UnhighlightSelection(); // Can't have 2 items selected!

        // Navigate up
        if (dir.y > 0)
        {
            optionsIndex--; // Decrement index

            // Set to highest (add act selectors!!)
            if (optionsIndex < 0) optionsIndex = numOptions[currentMenu] + 1;
        }
        // Navigate down
        else if (dir.y < 0)
        {
            // Increment by 1, then modulo by numOptions plus number of act selectors
            optionsIndex = (optionsIndex + 1) % (numOptions[currentMenu] + 2);
        }

        if(dir.x != 0) SelectItem(dir.x);

        HighlightSelection();   // Don't forget to have your item selected!
    }

    /// <summary>
    /// Sets what the currently referenced menu is. Useful
    /// when navigating forward through the branching menus.
    /// 
    /// Note: not comprehensive, please don't use unless you know other values will be properly aligned.
    /// </summary>
    /// <param name="currentMenu">Menu to set reference to.</param>
    public void SetCurrentMenu(CurrentMenu currentMenu)
    {
        UnhighlightSelection();

        this.currentMenu = currentMenu;
        optionsIndex = 0;

        HighlightSelection();
    }

    /// <summary>
    /// Sets what the currently referenced menu is. Useful
    /// when navigating forward through the branching menus.
    /// 
    /// Note: not comprehensive, please don't use unless you know other values will be properly aligned.
    /// </summary>
    /// <param name="currentMenu">Menu to set reference to.</param>
    public void SetCurrentMenu(int currentMenu)
    {
        SetCurrentMenu((CurrentMenu)currentMenu);
    }

    /// <summary>
    /// Utility function that ensures the previous option is not 
    /// highlighted.
    /// 
    /// optionsIndex MUST be in range [0, numOptions[currentMenu] + 1].
    /// </summary>
    void UnhighlightSelection()
    {
        if (printDebugs) Debug.Log("MenuController::UnhighlightSelection");

        // Regular option
        if (optionsIndex < numOptions[currentMenu])
        {
            switch (currentMenu)
            {
                case CurrentMenu.ObjectsSelection:
                    menuOptions[optionsIndex].UnhighlightOption();
                    break;
                default:
                    EventSystem.current.SetSelectedGameObject(null);    // Deselects whatever UI is selected
                    break;
            }
        }
        // Act selection - make the arrow stop glowing.
        else
        {
            int whichActArrow = (optionsIndex - numOptions[currentMenu]) % 2;     // will be either 0 or 1.

            // Up arrow
            if (whichActArrow == 0)
            {
                upConsoleButton.gameObject.GetComponent<GlowingItem>().StopGlowing();
                upConsoleBtnLight.color = originalLightColor;
            }
            // Down arrow
            else
            {
                downConsoleButton.gameObject.GetComponent<GlowingItem>().StopGlowing();
                downConsoleBtnLight.color = originalLightColor;
            }
        }
    }

    /// <summary>
    /// Utility function that just ensures that the proper UI
    /// option is highlighted.
    /// 
    /// optionsIndex MUST be in range [0, numOptions[currentMenu] + 1].
    /// </summary>
    void HighlightSelection()
    {
        if (printDebugs) Debug.Log("MenuController::HighlightSelection" +
            "\nCurrent Menu: " + currentMenu +
            "\nOptions Index: " + optionsIndex);

        // Regular option
        if (optionsIndex < numOptions[currentMenu])
        {
            Vector3 screenPoint;

            switch (currentMenu)
            {
                case CurrentMenu.ObjectsSelection:
                    menuOptions[optionsIndex].HighlightOption();
                    screenPoint = Camera.main.WorldToScreenPoint(menuOptions[optionsIndex].transform.position);
                    SetTargetRotation(screenPoint);
                    break;
                case CurrentMenu.PrimaryOptions:
                    primaryOptions[optionsIndex].Select();
                    break;
                case CurrentMenu.Settings:
                    settingsOptions[optionsIndex].Select();
                    break;
                case CurrentMenu.Audio:
                    audioOptions[optionsIndex].Select();
                    break;
                case CurrentMenu.Gameplay:
                    gameplayOptions[optionsIndex].Select();
                    break;
                case CurrentMenu.Graphics:
                    graphicsOptions[optionsIndex].Select();
                    break;
            }
        }
        // Act selection - make the arrow start glowing.
        else
        {
            int whichActArrow = (optionsIndex - numOptions[currentMenu]) % 2;     // will be either 0 or 1.

            if (whichActArrow == 0)
            {
                upConsoleButton.gameObject.AddComponent<GlowingItem>();  // Up arrow
                upConsoleBtnLight.color = highlightedLightColor;
            }
            else
            {
                downConsoleButton.gameObject.AddComponent<GlowingItem>();  // Down arrow
                downConsoleBtnLight.color = highlightedLightColor;
            }

            // Make camera look
            if (currentMenu == CurrentMenu.ObjectsSelection)
            {
                SetTargetRotation(new Vector2(2000, 2000));
            }
        }
    }

    /// <summary>
    /// Standard use for select item passing in a null for the sliderDir.
    /// </summary>
    void SelectItem()
    {
        SelectItem(null);
    }

    /// <summary>
    /// Performs an action appropriate for whatever the currently
    /// selected item is.
    /// </summary>
    void SelectItem(float? sliderDir)
    {
        if (printDebugs) Debug.Log("MenuController::SelectItem" +
            "\nCurrent Menu: " + currentMenu +
            "\nOptions Index: " + optionsIndex);

        // Handle sliders.
        if (sliderDir != null)
        {
            Slider currentSlider;

            switch (currentMenu)
            {
                case CurrentMenu.Audio:
                    currentSlider = audioOptions[optionsIndex].gameObject.GetComponent<Slider>();
                    if (currentSlider != null) currentSlider.value += sliderDir.Value * ((currentSlider.maxValue - currentSlider.minValue) / sliderIncrements);

                    break;
                case CurrentMenu.Gameplay:
                    currentSlider = gameplayOptions[optionsIndex].gameObject.GetComponent<Slider>();
                    if (currentSlider != null) currentSlider.value += sliderDir.Value * ((currentSlider.maxValue - currentSlider.minValue) / sliderIncrements);

                    break;
                case CurrentMenu.Graphics:
                    currentSlider = graphicsOptions[optionsIndex].gameObject.GetComponent<Slider>();
                    if (currentSlider != null) currentSlider.value += sliderDir.Value * ((currentSlider.maxValue - currentSlider.minValue) / sliderIncrements);

                    break;
                default: break;
            }

            return;
        }

        UnhighlightSelection(); // Some actions change the selected item.

        // Regular option
        if (optionsIndex < numOptions[currentMenu])
        {
            Button currentButton;
            Toggle currentToggle;

            switch (currentMenu)
            {
                case CurrentMenu.ObjectsSelection:
                    menuOptions[optionsIndex].SelectOption();
                    break;
                case CurrentMenu.PrimaryOptions:
                    currentButton = primaryOptions[optionsIndex].gameObject.GetComponent<Button>();
                    currentButton.onClick.Invoke();
                    break;
                case CurrentMenu.Settings:
                    currentButton = settingsOptions[optionsIndex].gameObject.GetComponent<Button>();
                    currentButton.onClick.Invoke();
                    break;
                case CurrentMenu.Audio:
                    currentButton = audioOptions[optionsIndex].gameObject.GetComponent<Button>();
                    if (currentButton != null) currentButton.onClick.Invoke();

                    currentToggle = audioOptions[optionsIndex].gameObject.GetComponent<Toggle>();
                    if (currentToggle != null) currentToggle.isOn = !currentToggle.isOn;

                    break;
                case CurrentMenu.Gameplay:
                    currentButton = gameplayOptions[optionsIndex].gameObject.GetComponent<Button>();
                    if (currentButton != null) currentButton.onClick.Invoke();

                    currentToggle = gameplayOptions[optionsIndex].gameObject.GetComponent<Toggle>();
                    if (currentToggle != null) currentToggle.isOn = !currentToggle.isOn;

                    break;
                case CurrentMenu.Graphics:
                    currentButton = graphicsOptions[optionsIndex].gameObject.GetComponent<Button>();
                    if (currentButton != null) currentButton.onClick.Invoke();

                    currentToggle = graphicsOptions[optionsIndex].gameObject.GetComponent<Toggle>();
                    if (currentToggle != null) currentToggle.isOn = !currentToggle.isOn;

                    break;
            }
        }
        // Act selection
        else
        {
            int whichActArrow = (optionsIndex - numOptions[currentMenu]) % 2;     // will be either 0 or 1.

            if (whichActArrow == 0) upConsoleButton.ClickButton();  // Up arrow
            else downConsoleButton.ClickButton();  // Down arrow
        }

        HighlightSelection();   // Ensure newly selected item is highlighted.
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
        ScenesManager.instance.nextSceneAfterWeaponSelect = ActList[currentActIndex].sceneEnumValue;
        ScenesManager.instance.LoadScene(Scenes.Loadout_Select);
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
        if (printDebugs) Debug.Log("MenuController::SetFullScreen");

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

    /// <summary>
    /// Finds the desired camera rotation based on mouse position.
    /// </summary>
    /// <param name="lookAtPosition">Position in screen space to look at.</param>
    /// <returns>Desired rotation in Euler angles.</returns>
    public void SetTargetRotation(Vector2 lookAtPosition)
    {
        // Get the mouse position
        float maxX = Screen.width;
        float midX = maxX / 2;
        float mouseX = Mathf.Max(Mathf.Min(lookAtPosition.x, maxX), 0);
        float rotation = lookAroundFreedom * Mathf.Sign(mouseX - midX) * Mathf.Pow(Mathf.Abs(((mouseX - midX) / midX)), 2);
        targetRotation = defaultCameraRotation + new Vector3(0f, rotation, 0f);

        if (crtAngleCamera == null) StartCoroutine(AngleTowardsTargetRotation());
    }

    /// <summary>
    /// Rotates the camera until its rotation matches the desired camera rotation.
    /// </summary>
    IEnumerator AngleTowardsTargetRotation()
    {
        while (true)
        {
            while (Camera.main.transform.eulerAngles != targetRotation)
            {
                float rotationDif = Math.Abs(Camera.main.transform.eulerAngles.y - targetRotation.y);   // Size of the rotation difference
                float direction = Math.Sign(targetRotation.y - Camera.main.transform.eulerAngles.y);    // Direction of the rotation difference
                float amntToRot = lookAroundSensitivity * Time.deltaTime;                               // Amount to rotate this frame

                // Rotate based on speed
                if (amntToRot < rotationDif) 
                    Camera.main.transform.eulerAngles = new Vector3(
                        defaultCameraRotation.x, 
                        Camera.main.transform.eulerAngles.y + amntToRot * direction, 
                        defaultCameraRotation.z);
                // Rotate rest of distance (less rotation than if went by speed)
                else Camera.main.transform.eulerAngles = targetRotation;

                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}