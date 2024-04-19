using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AbilitySelectUI : MonoBehaviour
{
    [SerializeField] private GameObject[] abilities;
    private HoverAndLerp2[] hoverAndLerps;
    public int Equipped;
    private int MaxNum;
    private GameObject inventoryParent;
    private AbilityInventory arsenal;

    private void Start()
    {
        inventoryParent = GameObject.Find("Player Inventory");
        arsenal = GameObject.Find("AbilityInventory").GetComponent<AbilityInventory>();
        MaxNum = arsenal.GetMaxAbilityCount();
        hoverAndLerps = new HoverAndLerp2[abilities.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            print(abilities[i].name);
            hoverAndLerps[i] = abilities[i].GetComponent<HoverAndLerp2>();
        }
    }
    public void CheckWeaponEquipLoad(HoverAndLerp2 hav)
    {
        if (Equipped < MaxNum || (hav.selected))
        {
            hav.EquipPass();
        }
        else { }//play sound or notify player too many weapons equipped
        //else notify user that too many equipped
    }
    public void setArseWeapon(Ability wep)
    {
        arsenal.AddAbilityToInventory(wep);
    }

    public void removeArseWeapon(Ability wep)
    {
        arsenal.RemoveAbilityByObject(wep);
    }

    public GameObject scanforhover()
    {
        foreach (GameObject ability in abilities)
        {
            if (ability.GetComponent<HoverAndLerp2>().IsMouseOver())
            {
                return ability;
            }
        }
        return null;
    }



}