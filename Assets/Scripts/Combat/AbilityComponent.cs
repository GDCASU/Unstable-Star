using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The ability component of the game, any entity can use it
/// </summary>
public class AbilityComponent : MonoBehaviour
{
    // NOTE: The way I set this up, abilites have their own cooldowns
    // meaning that if the player triggers one, they can still trigger the others

    // FIXME: Some abilites shouldnt be able to stack, like phase and proxibomb, but if 
    // the system is expanded to more abilities, a look up table would be better
    //private bool abilityLock; 

    // Settings
    [Header("Settings")]
    [SerializeField] private GameObject ModelObject;

    // Local Variables
    private MeshRenderer meshRenderer;
    private Material defaulMaterial;
    private CombatEntity entityComponent;

    // Set up
    public void Start()
    {
        // Get components
        meshRenderer = ModelObject.GetComponent<MeshRenderer>();
        entityComponent = GetComponent<CombatEntity>();

        // Set variables
        defaulMaterial = meshRenderer.material;
    }

    // Triggers the ability passed to it
    public void TriggerAbility(Ability inputAbility)
    {
        // If the passed ability is on cooldown, dont use it either
        if (inputAbility.isOnCooldown) return;
        
        // Else, determine what ability type it is
        switch(inputAbility.behaviour)
        {
            case AbilityTypes.PhaseShift:
                PerformPhaseShift(inputAbility);
                break;
            case AbilityTypes.ProxiBomb:
                //
                break;
            default:
                // Ability not recognized or not implemented
                Debug.Log("WARNING! ABILITY ENUM NOT DEFINED IN ABILITY COMPONENT!");
                break;
        }
    }

    // Function that triggers the effects of a phase ability
    private void PerformPhaseShift(Ability inputAbility)
    {
        // Start the ability
        StartCoroutine(PhaseShiftRoutine(inputAbility));

        // Start the cooldown of phase shift
        StartCoroutine(CooldownRoutine(inputAbility));
    }

    private IEnumerator PhaseShiftRoutine(Ability inputAbility)
    {
        // Entity becomes invulnerable but unable to hit others
        entityComponent.TriggerInvulnerability(inputAbility.durationTime, ignoreCollisions: true);

        // Lock Entity shooting
        entityComponent.LockShooting();

        // NOTE: If phase shift locks shooting, I guess it also locks other abilities
        entityComponent.LockAbilities();

        // Change the material of the model
        meshRenderer.material = inputAbility.PhaseShiftMaterial;

        // Wait until ability ends
        yield return new WaitForSeconds(inputAbility.durationTime);

        // Re-enable shooting
        entityComponent.UnlockShooting();

        // Re-enable Abilities
        entityComponent.UnlockAbilities();

        // Reset the material to default
        meshRenderer.material = defaulMaterial;
    }


    // Cooldown routine
    private IEnumerator CooldownRoutine(Ability input)
    {
        float timeLeft = input.cooldownTime;
        input.isOnCooldown = true;

        // Update time variable
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            input.timeLeftInCooldown = timeLeft;
            // Wait a frame
            yield return null;
        }

        // Cooldown ended
        input.timeLeftInCooldown = 0f;
        input.isOnCooldown = false;
    }


}
