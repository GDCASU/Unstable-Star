using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary> HACK: Temporary Reader of current player stats, not efficient but works </summary>
public class TempPlayerStatsReader : MonoBehaviour
{
    [Header("Player Object")]
    [SerializeField] private GameObject PlayerObj;

    //Local Variables
    private TMP_Text StatsReadoutText;
    private Player playerStats;

    private void Start()
    {
        playerStats = PlayerObj.GetComponent<Player>();
        StatsReadoutText = GetComponent<TMP_Text>();
    }

    // HACK: This is a pretty bad way of doing this, but it works for now until UI
    // Has a good framework set up
    private void Update()
    {
        //Set Text
        StatsReadoutText.text = "Shield: " + playerStats.GetShield() + "\n" + "Hull HP: " + playerStats.GetHealth();
    }
}
