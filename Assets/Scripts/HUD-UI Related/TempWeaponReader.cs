using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary> HACK: Temporary Reader of current player weapon, not efficient but works </summary>
public class TempWeaponReader : MonoBehaviour
{
    //Local Variables
    private TMP_Text WeaponReadoutText;
    private Player playerScript;

    private void Start()
    {
        GameObject PlayerObject = GameObject.Find("Player");

        WeaponReadoutText = GetComponent<TMP_Text>();
        playerScript = PlayerObject.GetComponent<Player>();
    }

    //HACK: This is a pretty bad way of doing this, but it works for now until UI
    //Has a good framework set up
    private void Update()
    {
        // If the player has no weapons, then just write "No Weapons"
        if (playerScript.GetCurrWeapon() == null)
        {
            WeaponReadoutText.text = "No Weapons";
            return;
        }
        WeaponReadoutText.text = playerScript.GetCurrWeapon().sName;
    }
}
