using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Scriptable Object that contains the data for a phase shift ability </summary>
[CreateAssetMenu(fileName = "ScriptablePhaseShift", menuName = "ScriptableObjects/Abilities/Phase Shift")]
public class ScriptablePhaseShift : ScriptableAbility
{
    // Variables
    [Header("Phase Shift Settings")]
    public string sName;
    public Material phaseShiftMaterial;
    public GameObject particlePrefab;
    public FMODUnity.EventReference phaseShiftEnter;
    public FMODUnity.EventReference phaseShiftExit;
    public Sprite abilityIconActive;
    public Sprite abilityIconInactive;
    public int charges;
    public float cooldownTime;
    public float durationTime;
    [TextAreaAttribute]
    public string description;
    public override Ability GetAbilityObject()
    {
        return new PhaseShiftAbility(sName, abilityIconActive, phaseShiftEnter, phaseShiftExit, abilityIconInactive, phaseShiftMaterial, particlePrefab, charges, cooldownTime, durationTime, description);
    }
}
