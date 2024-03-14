using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary> HACK: Temporary Reader of current player ability, not efficient but works </summary>
public class TempAbilityReadout : MonoBehaviour
{
    //Local Variables
    private TMP_Text AbilityReadoutText;
    private Ability currentAbility;

    private void Start()
    {
        AbilityReadoutText = GetComponent<TMP_Text>();
    }

    //HACK: This is a pretty bad way of doing this, but it works for now until UI
    //Has a good framework set up
    private void Update()
    {
        currentAbility = AbilityInventory.instance.GetCurrentAbility();

        // If the player has no abilities, then just write "No Ability"
        if (currentAbility == null)
        {
            AbilityReadoutText.text = "No Ability";
            return;
        }

        // Build readout
        string readout = currentAbility.sName + "\n";
        readout += "Charges Left = " + currentAbility.charges + "\n";
        if (currentAbility.isOnCooldown)
        {
            readout += "On Cooldown\n";
            readout += "Time Left = " + currentAbility.timeLeftInCooldown.ToString("F2");
        }
        else
        {
            readout += "Ready to use";
        }

        AbilityReadoutText.text = readout;
    }
}
