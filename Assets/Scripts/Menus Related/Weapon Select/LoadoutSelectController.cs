using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Script that manages everything to do with the Loadout select scene
/// </summary>
public class LoadoutSelectController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private Transform weaponCameraPoint;
    [SerializeField] private Transform abilityCameraPoint;
    [SerializeField] private GameObject buttonToAbilities;
    [SerializeField] private GameObject buttonToWeapons;
    [SerializeField] Button btnLaunch;
    [SerializeField] private float cameraMoveDuration;

    [Header("Global Disks Settings")]
    public float hoverHeight;
    public float hoverAnimDuration;

    [Header("HUD Settings")]
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private float warningFadeDuration;

    [Header("Screens")]
    public WeaponScreens weaponScreens;
    public AbilityScreens abilityScreens;

    [Header("Disks")]
    [SerializeField] List<SelectableDisk> weaponDisks;
    [SerializeField] List<SelectableDisk> abilityDisks;
    public List<SelectableDisk> currentDisks
    {
        get { return (inWeapons ? weaponDisks : abilityDisks); }
    }
    public int currentSelectionSize
    {
        get { return currentDisks.Count + 2; }
    }
    int currentSelection = 0;
    bool inWeapons = true;  // When false, in the abilities screen.

    [Header("Debugging")]
    [SerializeField] bool printDebugs = false;

    // Local fields
    private Coroutine warningTextFadeRoutine;

    #region Unity Events

    private void Start()
    {
        // Clear the player's Inventory before loading in
        WeaponArsenal.instance.ClearWeaponArsenal();
        AbilityInventory.instance.ClearAbilityInventory();

        // Correct for locked disks.
        for(int i = 0; i < weaponDisks.Count; i++)
        {
            if (!weaponDisks[i].IsDiskUnlocked()) weaponDisks.RemoveAt(i--);
        }

        for (int i = 0; i < abilityDisks.Count; i++)
        {
            if (!abilityDisks[i].IsDiskUnlocked()) abilityDisks.RemoveAt(i--);
        }

        // Attach to controls
        PlayerInput.OnMenuNavigate += Navigate;
        PlayerInput.OnSubmit += Select;
        PlayerInput.instance.ActivateUiControls();

        HighlightSelection();
    }

    private void OnDestroy()
    {
        // Remove hooks
        PlayerInput.OnMenuNavigate -= Navigate;
        PlayerInput.OnSubmit -= Select;
        PlayerInput.instance.ActivateShipControls();
    }

    #endregion

    /// <summary>
    /// Function used by the launch button to go into the next scene 
    /// if the player has at least one weapon equipped
    /// </summary>
    public void AttemptToLaunch()
    {
        if (!WeaponArsenal.instance.IsWeaponArsenalEmpty())
        {
            // Not empty, go into next scene
            ScenesManager.instance.LoadScene(ScenesManager.instance.nextSceneAfterWeaponSelect);
        }
        else
        {
            // Player hasnt selected at least one weapon, issue warning message
            if (warningTextFadeRoutine != null) StopCoroutine(warningTextFadeRoutine);
            warningTextFadeRoutine = StartCoroutine(WarningTextFade());
        }
    }

    /// <summary>
    /// Coroutine used to fade the warning text if issued
    /// </summary>
    private IEnumerator WarningTextFade()
    {
        float percentageComplete = 0;
        float elapsedTime = 0;
        Color fadingColor = new Color(warningText.color.r, warningText.color.g, warningText.color.b, 1);

        // Set warning text alpha value to 1
        warningText.color = fadingColor;

        // Lerp alpha value towards 0
        while (fadingColor.a > 0)
        {
            // Calculate step
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / warningFadeDuration;
            // Lerp
            fadingColor.a = Mathf.Lerp(fadingColor.a, 0, percentageComplete);
            // Set text Color
            warningText.color = fadingColor;
            // Wait a frame
            yield return null;
        }
    }

    #region Navigation

    /// <summary>
    /// Navigates through the menu.
    /// </summary>
    /// <param name="dir">Direction to navigate.</param>
    void Navigate(Vector2 dir)
    {
        UnhighlightSelection();

        // Increment
        if (dir.y > 0)
        {
            currentSelection = (currentSelection + 1) % currentSelectionSize;
        }
        // Decrement
        else if(dir.y < 0)
        {
            currentSelection--;
            if(currentSelection < 0) currentSelection = currentSelectionSize - 1;
        }

        HighlightSelection();
    }

    /// <summary>
    /// Activates the currently selected item.
    /// </summary>
    void Select()
    {
        UnhighlightSelection();

        // Not a disk
        if (currentSelection >= currentSelectionSize - 2)
        {
            // Arrow
            if (currentSelection - currentDisks.Count == 0)
            {
                if (inWeapons)
                {
                    buttonToAbilities.GetComponent<Button>().onClick?.Invoke();
                    inWeapons = false;
                }
                else
                {
                    buttonToWeapons.GetComponent<Button>().onClick?.Invoke();
                    inWeapons = true;
                }
            }
            else if (currentSelection - currentDisks.Count == 1) btnLaunch.onClick?.Invoke();    // Launch button
        }
        // Disk
        else currentDisks[currentSelection].Activate();

        HighlightSelection();
    }

    /// <summary>
    /// Makes the currently selected item more visible to the user.
    /// </summary>
    void HighlightSelection()
    {
        if (printDebugs) Debug.Log("LoadoutSelectController::HighlightSelection" +
            "\nCurrent Selection: " + currentSelection +
            "\nCurrent Selection Size: " + currentSelectionSize +
            "\nCurrent Disk Count: " + currentDisks.Count);

        // Not a disk
        if (currentSelection >= currentSelectionSize - 2)
        {
            // Arrow
            if (currentSelection - currentDisks.Count == 0)
            {
                if (printDebugs) Debug.Log("LoadoutSelectController::HighlightSelection::HighlightArrow");

                if (inWeapons) buttonToAbilities.GetComponent<Button>().Select();
                else buttonToWeapons.GetComponent<Button>().Select();
            }
            else if (currentSelection - currentDisks.Count == 1) btnLaunch.Select();    // Launch button
        }
        // Disk
        else currentDisks[currentSelection].Highlight();
    }

    /// <summary>
    /// Reverts the changes made to the currently selected item from HighlightSelection.
    /// </summary>
    void UnhighlightSelection()
    {
        if (currentSelection >= currentSelectionSize - 2) EventSystem.current.SetSelectedGameObject(null);  // Not a disk
        else currentDisks[currentSelection].Unhighlight();  // Disk
    }

    #endregion

    #region Camera Movement

    /// <summary>
    /// Function used by canvas buttons to move camera to ability screens
    /// </summary>
    public void MoveCameraToAbility()
    {
        StartCoroutine(MoveCamera(abilityCameraPoint.position, buttonToAbilities, buttonToWeapons));
    }

    /// <summary>
    /// Function used by canvas buttons to move camera to weapon screens
    /// </summary>
    public void MoveCameraToWeapon()
    {
        StartCoroutine(MoveCamera(weaponCameraPoint.position, buttonToWeapons, buttonToAbilities));
    }

    /// <summary>
    /// Coroutine that moves the camera either in front of the weapon select or the ability select
    /// </summary>
    /// <returns></returns>
    private IEnumerator MoveCamera(Vector3 targetPos, GameObject arrowToDisable, GameObject arrowToEnable)
    {
        float percentageComplete = 0;
        float elapsedTime = 0;

        // Enable arrow at original position (could be the left or right arrows)
        arrowToDisable.SetActive(false);

        // Lerp Camera towards target
        while (Camera.main.transform.position != targetPos)
        {
            // Calculate step
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / cameraMoveDuration;
            // Lerp
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, percentageComplete);
            // Wait a frame
            yield return null;
        }

        // Enable arrow at target (could be the left or right)
        arrowToEnable.SetActive(true);
    }

    #endregion

    #region Screens

    /// <summary>
    /// Class that contains all data necessary to interface with the weapon screens
    /// </summary>
    [System.Serializable]
    public class WeaponScreens
    {
        [Header("Canvas Objects")]
        [SerializeField] private TextMeshProUGUI wepNameText;
        [SerializeField] private TextMeshProUGUI wepDescText;
        [SerializeField] private TextMeshProUGUI wepDmgText;
        [SerializeField] private Image weaponIcon;

        [Header("Text Settings")]
        [SerializeField] private string dmgTextLeftSide;
        [SerializeField] private string nameTextLeftSide;

        /// <summary>
        /// Sets the fields of the screens
        /// </summary>
        public void SetFields(Weapon weapon)
        {
            wepNameText.text = nameTextLeftSide + weapon.sName;
            wepDescText.text = weapon.description;
            wepDmgText.text = dmgTextLeftSide + weapon.damage;
            weaponIcon.sprite = weapon.weaponIcon;
            weaponIcon.color = new Color(weaponIcon.color.r, weaponIcon.color.g, weaponIcon.color.b, 1);
        }

        /// <summary>
        /// Sets text fields to empty
        /// </summary>
        public void ClearFields()
        {
            wepNameText.text = string.Empty;
            wepDescText.text = string.Empty;
            wepDmgText.text = string.Empty;
            weaponIcon.color = new Color(weaponIcon.color.r, weaponIcon.color.g, weaponIcon.color.b, 0);
        }
    }

    /// <summary>
    /// Class that contains all data necessary to interface with the ability screens
    /// </summary>
    [System.Serializable]
    public class AbilityScreens
    {
        [Header("Canvas Objects")]
        [SerializeField] private TextMeshProUGUI abilityNameText;
        [SerializeField] private TextMeshProUGUI abilityDescText;
        [SerializeField] private TextMeshProUGUI abilityChargesText;
        [SerializeField] private Image abilityIcon;

        [Header("Text Settings")]
        [SerializeField] private string chargesTextLeftSide;
        [SerializeField] private string nameTextLeftSide;

        /// <summary>
        /// Sets the fields of the screens
        /// </summary>
        public void SetFields(Ability ability)
        {
            abilityNameText.text = nameTextLeftSide + ability.sName;
            abilityDescText.text = ability.description;
            abilityChargesText.text = chargesTextLeftSide + ability.charges;
            abilityIcon.sprite = ability.abilityIconActive;
            abilityIcon.color = new Color(abilityIcon.color.r, abilityIcon.color.g, abilityIcon.color.b, 1);
        }

        /// <summary>
        /// Sets text fields to empty
        /// </summary>
        public void ClearFields()
        {
            abilityNameText.text = string.Empty;
            abilityDescText.text = string.Empty;
            abilityChargesText.text = string.Empty;
            abilityIcon.color = new Color(abilityIcon.color.r, abilityIcon.color.g, abilityIcon.color.b, 0);
        }
    }

    #endregion
}

