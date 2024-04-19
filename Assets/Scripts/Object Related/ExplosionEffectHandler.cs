using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script to delete the explosion object once it finishes animating
/// </summary>
public class ExplosionEffectHandler : MonoBehaviour
{
    void Start()
    {
        // Get components
        ParticleSystem particleSystemComponent = GetComponent<ParticleSystem>();
        float duration = particleSystemComponent.main.duration;

        // Start Destroy Routine
        StartCoroutine(ObjectDestroyTimer(duration));
    }

    // Will call destroy on this object once the explosion effect finishes
    private IEnumerator ObjectDestroyTimer(float duration)
    {
        // Add a small delta to make sure effect completes
        yield return new WaitForSeconds(duration + 0.3f);

        // Call destroy
        Destroy(gameObject);
    }

    
}
