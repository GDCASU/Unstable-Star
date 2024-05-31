using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSelectUI : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    private HoverAndLerp[] hoverAndLerps;
    public int Equipped;
    private int MaxNum;

    private void Start()
    {
        MaxNum = WeaponArsenal.instance.GetMaxArsenalCount();
        hoverAndLerps = new HoverAndLerp[weapons.Length];
        for (int i = 0; i < weapons.Length; i++)
        {
            print(weapons[i].name);
            hoverAndLerps[i] = weapons[i].GetComponent<HoverAndLerp>();
        }
    }
    public void CheckWeaponEquipLoad(HoverAndLerp hav)
    {
        if (Equipped < MaxNum || (hav.selected))
        {
            hav.EquipPass();
        }
        else { }//play sound or notify player too many weapons equipped
        //else notify user that too many equipped
    }
    public void setArseWeapon(Weapon wep)
    {
        WeaponArsenal.instance.AddWeaponToArsenal(wep);
    }

    public void removeArseWeapon(Weapon wep)
    {
        WeaponArsenal.instance.RemoveWeaponByObject(wep);
    }

    public GameObject scanforhover()
    {
        foreach(GameObject gun in weapons)
        {
            if(gun.GetComponent<HoverAndLerp>().IsMouseOver())
            {
                return gun;
            }
        }
        return null;
    }



}
