using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Simple utility script to add weapons to the array outside of gameplay routine
// Add this to an empty object on the world and modify it to change the loadout
public class AddWeaponsUtil : MonoBehaviour
{
    // This will hold the DataObject for the weapon Loadout
    [SerializeField] private GameObject weaponArsenalObject;
    private WeaponArsenal weaponLoadout;

    // Local Variables
    public bool doAddWeapon = false;
    public bool doClearLoadout = false;
    public bool setCurrWpToFirst = false;
    public bool doRmPistol = false;
    public bool doRmBuckshot = false;
    public int targetIndex = 0;
    public bool doRmTargetIndx = false;
    public bool doSwitchToIndex = false;
    Weapon pistol;
    Weapon birdshot;
    Weapon buckshot;

    private void Start()
    {
        weaponLoadout = weaponArsenalObject.GetComponent<WeaponArsenal>();
        pistol = weaponLoadout.PlayerPistol.GetWeaponObject();
        birdshot = weaponLoadout.PlayerBirdshot.GetWeaponObject();
        buckshot = weaponLoadout.PlayerBuckshot.GetWeaponObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (doAddWeapon)
        {
            AddWeapon(pistol);
            AddWeapon(birdshot);
            AddWeapon(buckshot);
            doAddWeapon = false;
        }
        // Clear the stored weapons
        if (doClearLoadout)
        {
            weaponLoadout.ClearWeaponArsenal();
            doClearLoadout = false;
        }
        if (doRmPistol)
        {
            RemoveWeaponObject(pistol);
            doRmPistol = false;
        }
        if (doRmBuckshot)
        {
            RemoveWeaponObject(buckshot);
            doRmBuckshot = false;
        }
        if (doRmTargetIndx)
        {
            RemoveWeaponIndex(targetIndex);
            doRmTargetIndx = false;
        }
        if (setCurrWpToFirst)
        {
            SetWeaponToFirst();
            setCurrWpToFirst = false;
        }
    }

    // Adding target Weapon
    public void AddWeapon(Weapon weapon)
    {
        weaponLoadout.AddWeaponToArsenal(weapon);
    }

    // Removing weapon specified
    public void RemoveWeaponObject(Weapon target)
    {
        weaponLoadout.RemoveWeaponByObject(target);
    }

    // Removing index specified
    public void RemoveWeaponIndex(int index)
    {
        weaponLoadout.RemoveWeaponByIndex(index);
    }

    // Set current weapon to the first
    public void SetWeaponToFirst()
    {
        weaponLoadout.SetCurrentWeaponToFirst();
    }
}
