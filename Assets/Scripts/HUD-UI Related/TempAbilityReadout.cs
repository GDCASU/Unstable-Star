using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary> HACK: Temporary Reader of current player ability, not efficient but works </summary>
public class TempAbilityReadout : MonoBehaviour
{
    //Local Variables
    private TMP_Text AbilityReadoutText;
    private Player playerScript;
    private Ability currentAbility;

    private void Start()
    {
        GameObject PlayerObject = GameObject.Find("Player");

        AbilityReadoutText = GetComponent<TMP_Text>();
        playerScript = PlayerObject.GetComponent<Player>();
    }

    //HACK: This is a pretty bad way of doing this, but it works for now until UI
    //Has a good framework set up
    private void Update()
    {
        currentAbility = playerScript.GetCurrAbility();

        // If the player has no abilities, then just write "No Ability"
        if (currentAbility == null)
        {
            AbilityReadoutText.text = "No Ability";
            return;
        }

        // Build readout
        string readout = playerScript.GetCurrAbility().sName + "\n";
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
