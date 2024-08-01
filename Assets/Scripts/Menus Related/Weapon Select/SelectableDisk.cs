using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that managed the floppy disks on the loadout scene
/// </summary>
public class SelectableDisk : MonoBehaviour
{
    // Helper enum to make the script adaptable to both weapons and abilities
    private enum LoadoutType
    {
        Weapon,
        Ability
    }

    [Header("Disk Settings")]
    [SerializeField] private LoadoutSelectController _loadoutController;
    [SerializeField] private LoadoutType _loadoutType;
    [SerializeField] private ScriptableObject _loadoutObject;
    [SerializeField] private Material _transparentDiskMat;
    [SerializeField] private GameObject _selectionFlagObj;

    // Local Fields
    private Coroutine _hoverRoutine;
    private Vector3 _originalPos;
    private Vector3 _endPos;
    private MeshRenderer _meshRenderer;
    private Material _originalMat;
    private bool _isSelected;
    private bool _isUnlocked;

    // HACK: Store a reference to the weapon/ability to avoid excessive casting
    private Weapon diskWeapon;
    private Ability diskAbility;

    #region Unity Events

    private void Awake()
    {
        // Set fields
        _meshRenderer = GetComponent<MeshRenderer>();
        _originalMat = _meshRenderer.material;
        _originalPos = transform.position;
        _isSelected = false;
        _isUnlocked = false;
        _endPos = transform.position + _loadoutController.hoverHeight * transform.up;
    }

    private void Start()
    {
        IsDiskUnlocked();
    }

    // Will trigger when the mouse hovers over its collider
    private void OnMouseEnter()
    {
        Highlight();
    }

    // Will trigger when the mouse stops hovering over its collider
    private void OnMouseExit()
    {
        Unhighlight();
    }

    // Will trigger when the mouse clicks while over its collider
    private void OnMouseDown()
    {
        Activate();
    }

    #endregion

    #region Navigation

    /// <summary>
    /// Makes it clear to the user which disk is currently being looked at.
    /// </summary>
    public void Highlight()
    {
        // Dont do anything if locked
        if (!_isUnlocked) return;

        // Handle Coroutine
        if (_hoverRoutine != null) StopCoroutine(_hoverRoutine);
        _hoverRoutine = StartCoroutine(HoverDisk(_endPos));

        // Set the screen text
        switch (_loadoutType)
        {
            case LoadoutType.Weapon:
                _loadoutController.weaponScreens.SetFields(diskWeapon);
                break;

            case LoadoutType.Ability:
                _loadoutController.abilityScreens.SetFields(diskAbility);
                break;

            default:
                Debug.Log("<color=red> ERROR! LoadoutType not defined in SelectableDisk::OnMouseEnter at object = " + gameObject.name + "</color>");
                return;
        }

        // Play disk raised sound
        SoundManager.instance.PlaySound(_loadoutController.diskRaised);
    }

    /// <summary>
    /// Removes changes made by the Highlight function.
    /// </summary>
    public void Unhighlight() {
        // Dont do anything if locked
        if (!_isUnlocked) return;

        // Handle Coroutine
        if (_hoverRoutine != null) StopCoroutine(_hoverRoutine);
        _hoverRoutine = StartCoroutine(HoverDisk(_originalPos));

        // Clear the screen text
        switch (_loadoutType)
        {
            case LoadoutType.Weapon:
                _loadoutController.weaponScreens.ClearFields();
                break;

            case LoadoutType.Ability:
                _loadoutController.abilityScreens.ClearFields();
                break;

            default:
                Debug.Log("<color=red> ERROR! LoadoutType not defined in SelectableDisk::OnMouseExit at object = " + gameObject.name + "</color>");
                return;
        }
    }

    /// <summary>
    /// Performs the action of this item.
    /// </summary>
    public void Activate()
    {
        // Dont do anything if locked
        if (!_isUnlocked)
        {
            // Play error sound if not unlocked
            SoundManager.instance.PlaySound(_loadoutController.failedAction);
            return;
        }

        // Attempt to add or remove selected disk to loadout
        switch (_loadoutType)
        {
            case LoadoutType.Weapon:
                AttemptToToggleWeapon();
                break;

            case LoadoutType.Ability:
                AttemptToToggleAbility();
                break;

            default:
                Debug.Log("<color=red> ERROR! LoadoutType not defined in SelectableDisk::OnMouseDown at object = " + gameObject.name + "</color>");
                return;
        }
    }

    #endregion

    /// <summary>
    /// Will attempt to add/Remove the weapon disk that was clicked
    /// </summary>
    private void AttemptToToggleWeapon()
    {
        if (_isSelected)
        {
            // It was present in the weapon arsenal, remove it
            bool wasRemoved = WeaponArsenal.instance.RemoveWeaponByObject(diskWeapon);

            if (!wasRemoved)
            {
                // Failed to remove from inventory even if it was selected
                string msg = "ERROR! Failed to remove the weapon from the arsenal\n";
                msg += "Error ocurred on " + gameObject.name;
                Debug.Log(msg);
                return;
            }
            // Removed successfully, set flag
            _selectionFlagObj.SetActive(false);
            _isSelected = false;

            // Play clicked sound
            SoundManager.instance.PlaySound(_loadoutController.diskClicked);
            return;
        }

        // Else, wasnt selected, attempt to add it to arsenal
        bool wasAdded = WeaponArsenal.instance.AddWeaponToArsenal(diskWeapon);
        if (wasAdded)
        {
            // Weapon was added successfully, set the flag
            _selectionFlagObj.SetActive(true);
            _isSelected = true;

            // Play clicked sound
            SoundManager.instance.PlaySound(_loadoutController.diskClicked);
        }
        else
        {
            // Wasnt added, play error sound
            SoundManager.instance.PlaySound(_loadoutController.failedAction);
        }
    }

    /// <summary>
    /// Will attempt to add/remove the ability disk that was clicked
    /// </summary>
    private void AttemptToToggleAbility()
    {
        if (_isSelected)
        {
            // It was present in the ability inventory, remove it
            bool wasRemoved = AbilityInventory.instance.RemoveAbilityByObject(diskAbility);

            if (!wasRemoved)
            {
                // Failed to remove from inventory even if it was selected
                string msg = "ERROR! Failed to remove the ability from the inventory\n";
                msg += "Error ocurred on " + gameObject.name;
                Debug.Log(msg);
                return;
            }
            // Removed successfully, set flag
            _selectionFlagObj.SetActive(false);
            _isSelected = false;

            // Play clicked sound
            SoundManager.instance.PlaySound(_loadoutController.diskClicked);
            return;
        }

        // Else, wasnt selected, attempt to add it to inventory
        bool wasAdded = AbilityInventory.instance.AddAbilityToInventory(diskAbility);
        if (wasAdded)
        {
            // Ability was added successfully, set the flag
            _selectionFlagObj.SetActive(true);
            _isSelected = true;

            // Play clicked sound
            SoundManager.instance.PlaySound(_loadoutController.diskClicked);
        }
        else
        {
            // Wasnt added, play error sound
            SoundManager.instance.PlaySound(_loadoutController.failedAction);
        }
    }

    /// <summary>
    /// Checks if the disk is unlocked.
    /// </summary>
    /// <returns>Whether the disk is unlocked or not.</returns>
    public bool IsDiskUnlocked()
    {
        // Check if passed scriptable object is unlocked or not
        switch (_loadoutType)
        {
            case LoadoutType.Weapon:
                // Check for weapon unlocked
                return IsWeaponUnlocked();

            case LoadoutType.Ability:
                // Check for ability unlocked
                return IsAbilityUnlocked();

            default:
                Debug.Log("<color=red> ERROR: Loadout Type not found at SelectableDisk::Start on GameObject = " + gameObject.name + "</color>");
                return false;
        }
    }

    /// <summary>
    /// Checks if the weapon passed is unlocked
    /// </summary>
    private bool IsWeaponUnlocked()
    {
        // Its a weapon, first cast the scriptable object
        ScriptableWeapon scriptWeapon = (ScriptableWeapon)_loadoutObject;
        // Now get the weapon stored in it
        Weapon weapon = scriptWeapon.GetWeaponObject();

        // Check if unlocked on the save file
        switch (weapon.weaponType)
        {
            case WeaponTypes.Pistol: // Always unlocked
                _isUnlocked = true;
                break;

            case WeaponTypes.Laser:
                _isUnlocked = SerializedDataManager.instance.gameData.isLaserUnlocked;
                break;

            case WeaponTypes.Gatling:
                _isUnlocked = SerializedDataManager.instance.gameData.isGatlingUnlocked;
                break;

            case WeaponTypes.Birdshot:
                // NOTE: NOT IMPLEMENTED
                break;

            case WeaponTypes.Buckshot:
                // NOTE: NOT IMPLEMENTED
                break;

            default:
                // Weapon wasnt found on file or isnt implemented
                Debug.Log("<color=red> ERROR: Weapon type not found on SelectableDisk::isWeaponUnlocked on GameObject = " + gameObject.name + "</color>");
                return false;
        }

        // Set the material to transparent if not unlocked
        if (!_isUnlocked)
        {
            _meshRenderer.material = _transparentDiskMat;
        }

        // Set the weapon field to the extracted weapon
        diskWeapon = weapon;

        return _isUnlocked;
    }

    /// <summary>
    /// Checks if the ability passed is unlocked
    /// </summary>
    private bool IsAbilityUnlocked()
    {
        // Its an ability, first cast the scriptable object
        ScriptableAbility scriptAbility = (ScriptableAbility)_loadoutObject;
        // Now get the ability stored in it
        Ability ability = scriptAbility.GetAbilityObject();

        // Check if unlocked on the save file
        switch (ability.abilityType)
        {
            case AbilityTypes.PhaseShift:
                _isUnlocked = SerializedDataManager.instance.gameData.isPhaseShiftUnlocked;
                break;

            case AbilityTypes.ProxiBomb:
                _isUnlocked = SerializedDataManager.instance.gameData.isProxiBombUnlocked;
                break;

            default:
                // Ability wasnt found on file or isnt implemented
                Debug.Log("<color=red> ERROR: Ability type not found on SelectableDisk::isAbilityUnlocked on GameObject = " + gameObject.name + "</color>");
                return false;
        }

        // Set the material to transparent if not unlocked
        if (!_isUnlocked)
        {
            _meshRenderer.material = _transparentDiskMat;
        }

        // Set the ability field to the extracted weapon
        diskAbility = ability;

        return _isUnlocked;
    }

    private IEnumerator HoverDisk(Vector3 targetPos)
    {
        float percentageComplete = 0;
        float elapsedTime = 0;

        // FIXME: ADJUST TIME OF ANIMATION DEPENDING ON DISTANCE?

        // Lerp disk towards target
        while (transform.position != targetPos)
        {
            // Calculate step
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / _loadoutController.hoverAnimDuration;
            // Lerp
            transform.position = Vector3.Lerp(transform.position, targetPos, percentageComplete);
            // Wait a frame
            yield return null;
        }
    }

}
