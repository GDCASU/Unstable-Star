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
    private string readout;
    private Weapon playerWep;

    private void Start()
    {
        WeaponReadoutText = GetComponent<TMP_Text>();
        playerScript = Player.Instance.gameObject.GetComponent<Player>();

        // Default text
        readout = "No Weapons";
        WeaponReadoutText.text = readout;
    }

    //HACK: This is a pretty bad way of doing this, but it works for now until UI
    //Has a good framework set up
    private void Update()
    {
        // If the player has no weapons, then just write "No Weapons"
        if (playerScript.GetCurrWeapon() == null) return;

        // Else, get weapon name
        playerWep = Player.Instance.GetCurrWeapon();
        readout = playerWep.sName + "\n";

        // Check for cooldown
        if (playerWep.timeLeftInCooldown > 0)
        {
            readout += "On Cooldown\n";
            readout += "Time left = " + playerWep.timeLeftInCooldown.ToString("F2");
        }
        else
        {
            readout += "Ready to use";
        }

        // Set Text
        WeaponReadoutText.text = readout;
    }
}
