using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Local fields
    private Coroutine warningTextFadeRoutine;


    private void Start()
    {
        // Clear the player's Inventory before loading in
        WeaponArsenal.instance.ClearWeaponArsenal();
        AbilityInventory.instance.ClearAbilityInventory();
    }

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
    /// <returns></returns>
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

