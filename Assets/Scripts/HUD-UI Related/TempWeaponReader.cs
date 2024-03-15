using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

/// <summary> HACK: Temporary Reader of current player weapon, not efficient but works </summary>
public class TempWeaponReader : MonoBehaviour
{
    //Local Variables
    private TMP_Text WeaponReadoutText;
    private string readout;
    private Weapon playerWep;

    private void Start()
    {
        WeaponReadoutText = GetComponent<TMP_Text>();

        // Default text
        readout = "No Weapons";
        WeaponReadoutText.text = readout;
    }

    //HACK: This is a pretty bad way of doing this, but it works for now until UI
    //Has a good framework set up
    private void Update()
    {
        //Get weapon info
        playerWep = WeaponArsenal.instance.GetCurrentWeapon();

        // If the player has no weapons, then just write "No Weapons"
        if (playerWep == null) return;

        // Figure out how to set text
        switch(playerWep.weaponType)
        {
            case WeaponTypes.Gatling:
                UpdateForGatling(playerWep);
                break;
            case WeaponTypes.Laser:
                UpdateForLaser(playerWep);
                break;
            default:
                UpdateForSimpleWeapon(playerWep);
                break;
        }

        // Set text
        WeaponReadoutText.text = readout;
    }

    private void UpdateForSimpleWeapon(Weapon input)
    {
        // Get weapon name
        readout = playerWep.sName + "\n";

        // Check for cooldown
        if (playerWep.timeLeftInCooldown > 0)
        {
            readout += "On Cooldown\n";
            readout += "Time left = " + playerWep.timeLeftInCooldown.ToString("F2");
        }
        else if (playerWep.timeLeftInCooldown <= 0)
        {
            readout += "Ready to use!";
        }
    }

    private void UpdateForGatling(Weapon input)
    {
        // Get weapon name
        readout = playerWep.sName + "\n";

        // Check if we are warming up
        if (input.warmupCounter > 0f)
        {
            readout += "Warming Up!\n";
            readout += "Time Left = " + input.warmupCounter.ToString("F2");
        }
        // Else, we are firing as long as button is held
        else if (PlayerInput.instance.isShootHeld && !Player.Instance.isShootingLocked)
        {
            readout += "Firing!";
        }
        // Not firing
        else
        {
            readout += "Ready!";
        }
    }

    private void UpdateForLaser(Weapon input)
    {
        // Get weapon name
        readout = playerWep.sName + "\n";

        // Check if we are charging the laser
        bool isInputGood = PlayerInput.instance.isShootHeld && !Player.Instance.isShootingLocked;
        bool laserChargingCheck = isInputGood && input.chargeTimeCounter < input.maxChargeUpTime && !input.isOnCooldown;
        if (laserChargingCheck)
        {
            readout += "Charging Up!\n";
            readout += "Time Charged = " + input.chargeTimeCounter.ToString("F2");
        }
        // Else, we are fully charged
        else if (isInputGood && input.chargeTimeCounter >= input.maxChargeUpTime)
        {
            readout += "Max Charge!";
        }
        // On cooldown
        else if (input.timeLeftInCooldown > 0)
        {
            readout += "On Cooldown!\n";
            readout += "Time Left = " + input.timeLeftInCooldown.ToString("F2");
        }
        // Not firing or locked
        else
        {
            readout += "Ready!";
        }
    }
}
