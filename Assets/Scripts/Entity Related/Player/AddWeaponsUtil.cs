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
    private WeaponLoadout weaponLoadout;

    // Local Variables
    public bool doAddWeapon = false;
    public bool doClearLoadout = false;
    public bool setCurrWpToFirst = false;
    public bool doRmPistol = false;
    public bool doRmBuckshot = false;
    public int targetIndex = 0;
    public bool doRmTargetIndx = false;
    public bool doSwitchToIndex = false;
    Pistol pistol;
    Birdshot birdshot;
    Buckshot buckshot;

    private void Start()
    {
        pistol = new Pistol(BulletColors.Red, 30f, 1, "Pistol", 0.2f);
        birdshot = new Birdshot(BulletColors.Green ,30f, 1, "Birdshot", 0.2f);
        buckshot = new Buckshot(BulletColors.Yellow, 30f, 1, "Buckshot", 0.2f);
        weaponLoadout = weaponArsenalObject.GetComponent<WeaponLoadout>();
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
            weaponLoadout.ClearWeaponLoadout();
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
        if (doSwitchToIndex)
        {
            SetCurrWeaponToIndex(targetIndex);
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

    // Set current weapon to an index
    public void SetCurrWeaponToIndex(int index)
    {
        weaponLoadout.SetCurrentWeaponToIndex(index);
    }
}
