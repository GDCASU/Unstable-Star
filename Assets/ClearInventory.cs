using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearInventory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WeaponArsenal arsenal = WeaponArsenal.instance;
        arsenal.ClearWeaponArsenal();

        AbilityInventory abilityInventory = AbilityInventory.instance;
        abilityInventory.ClearAbilityInventory();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
