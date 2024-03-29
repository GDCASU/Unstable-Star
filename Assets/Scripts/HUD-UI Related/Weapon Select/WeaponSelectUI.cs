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
    private GameObject inventoryParent;
    private WeaponArsenal arsenal;

    private void Start()
    {
        inventoryParent = GameObject.Find("Player Inventory");
        arsenal = GameObject.Find("Weapon Arsenal").GetComponent<WeaponArsenal>();
        MaxNum = arsenal.GetMaxArsenalCount();
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
        arsenal.AddWeaponToArsenal(wep);
    }

    public void removeArseWeapon(Weapon wep)
    {
        arsenal.RemoveWeaponByObject(wep);
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
