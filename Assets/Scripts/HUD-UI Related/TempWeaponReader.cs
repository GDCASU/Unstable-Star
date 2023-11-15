using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary> HACK: Temporary Reader of current player weapon, not efficient but works </summary>
public class TempWeaponReader : MonoBehaviour
{
    [Header("Player Object")]
    [SerializeField] private GameObject PlayerObj;

    //Local Variables
    private TMP_Text WeaponReadoutText;
    private Shoot playerShootScript;

    private void Start()
    {
        WeaponReadoutText = GetComponent<TMP_Text>();
        playerShootScript = PlayerObj.GetComponent<Shoot>();
    }

    //HACK: This is a pretty bad way of doing this, but it works for now until UI
    //Has a good framework set up
    private void Update()
    {
        //Set Text
        WeaponReadoutText.text = playerShootScript.GetCurrWeapon().GetName();
    }
}
