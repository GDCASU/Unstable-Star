using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the data for a phase shift ability </summary>
[CreateAssetMenu(fileName = "ScriptablePhaseShift", menuName = "ScriptableObjects/Abilities/Phase Shift")]
public class ScriptablePhaseShift : ScriptableObject
{
    // Variables
    [Header("Phase Shift Settings")]
    public string sName;
    public Material phaseShiftMaterial;
    public float cooldownTime;
    public float durationTime;
    

    /// <summary>
    /// Helper function to construct an Ability object from stored data
    /// </summary>
    public Ability GetAbilityObject()
    {
        return new PhaseShiftAbility(sName, phaseShiftMaterial, cooldownTime, durationTime);
    }
}
