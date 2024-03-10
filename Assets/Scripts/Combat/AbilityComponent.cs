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
        
        // Else, determine what ability type it is and call its function
        switch(inputAbility.behaviour)
        {
            case AbilityTypes.PhaseShift:
                PerformPhaseShift(inputAbility);
                break;
            case AbilityTypes.ProxiBomb:
                PerformProximityBomb(inputAbility);
                break;
            default:
                // Ability not recognized or not implemented
                Debug.Log("WARNING! ABILITY ENUM NOT DEFINED IN ABILITY COMPONENT!");
                break;
        }
    }

    #region PHASE SHIFT

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

        // Change the material of the model and create the particles
        meshRenderer.material = inputAbility.PhaseShiftMaterial;

        // Create the particles
        GameObject particleEmitter = Instantiate(inputAbility.particleEmitter);
        particleEmitter.transform.position = this.transform.position;

        // Wait until ability ends
        yield return new WaitForSeconds(inputAbility.durationTime);

        // Destroy the particle object
        Destroy(particleEmitter);

        // Re-enable shooting
        entityComponent.UnlockShooting();

        // Re-enable Abilities
        entityComponent.UnlockAbilities();

        // Reset the material to default
        meshRenderer.material = defaulMaterial;

        // Create the particles again once phase shift ends
        particleEmitter = Instantiate(inputAbility.particleEmitter);
        particleEmitter.transform.position = this.transform.position;

        // Destroy the object after waiting a bit
        yield return new WaitForSeconds(2f);
        Destroy(particleEmitter);
    }

    #endregion

    #region PROXIMITY BOMB

    // Proximity bomb
    private void PerformProximityBomb(Ability inputAbility)
    {
        // Variables
        GameObject bomb;
        ProximityBomb bombScript;
        // Create the proxibomb and set its data
        bomb = Instantiate(inputAbility.bombPrefab);
        // Set the bomb at the position of its creator
        bomb.transform.position = this.transform.position;
        // Set its data
        bombScript = bomb.GetComponent<ProximityBomb>();
        bombScript.StartBomb(inputAbility, this.gameObject);
        // Start its cooldown
        StartCoroutine(CooldownRoutine(inputAbility));
    }

    #endregion

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
