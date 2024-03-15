using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// The proximity bomb class, it has to be started by its caller
/// </summary>
public class ProximityBomb : MonoBehaviour
{
    // Settings
    private readonly float damageDuration = 1.5f;
    private readonly float fadeDuration = 1f;

    // Data
    private Ability bombInfo;
    private MeshRenderer meshRenderer;

    // Local Variables
    private Dictionary<int, CombatEntity> collisionHistory = new();
    private float elapsedTime = 0f;
    private bool isDamageDisabled = false;

    /// <summary> 
    /// Method called by its creator to start the bomb <para />
    /// Pass the creator object into the caller argument
    /// </summary>
    public void StartBomb(Ability inputAbility, GameObject caller)
    {
        // Set creator info and add the creator to the list of ignores
        bombInfo = inputAbility;
        collisionHistory.Add(caller.GetInstanceID(), null);

        // Get the components
        meshRenderer = GetComponent<MeshRenderer>();

        // HACK: its a pain to check static on static collisions in unity,
        // And for some reason even OverlapSphere doesnt work properly,
        // so we make the bomb "Vibrate" to promote checking
        StartCoroutine(Vibrate());

        // Start the bomb routine
        StartCoroutine(ProxiBombRoutine());
    }

    // Expands the Hitbox sphere to collide with entities
    private IEnumerator ProxiBombRoutine()
    {
        // Since the scaler in unity is the diameter of the sphere, we multiply the bomb radius by 2
        float size = bombInfo.bombRadius * 2;
        Vector3 targetSize = new Vector3(size, size, size);

        // Start lerping towards target size
        float percentageComplete;
        while (this.transform.localScale.x < targetSize.x)
        {
            // Calculate step
            elapsedTime += Time.deltaTime;
            percentageComplete = elapsedTime / damageDuration;
            // Lerp
            this.transform.localScale = Vector3.Lerp(transform.localScale, targetSize, percentageComplete);
            // Wait a frame
            yield return null;
        }

        // Disable damage
        isDamageDisabled = true;

        // Fade away bomb by slowly moving the contrast to 0
        elapsedTime = 0f;
        string sContrast = "_Contrast";
        float contrastMax = meshRenderer.material.GetFloat(sContrast);
        float rateOfChange = contrastMax / fadeDuration;
        float currentVal;
        // Start Fading
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            // Modify the transparency of the material
            currentVal = contrastMax - rateOfChange * elapsedTime;
            meshRenderer.material.SetFloat(sContrast, currentVal);

            // Wait a frame
            yield return null;
        }

        // Destroy the object upon finishing
        Destroy(gameObject);
    }

    // Function to vibrate bomb to promote collision checks
    private IEnumerator Vibrate()
    {
        float speed = 0.001f;
        Vector3 dirVec = new Vector3(1,1,0);
        Vector3 movementVec;
        // Move bomb
        while (true)
        {
            movementVec = speed * Time.deltaTime * dirVec;
            dirVec *= -1;
            transform.Translate(movementVec); 
            yield return null;
        }
    }

    // Check for collisions
    private void OnCollisionEnter(Collision collision)
    {
        // Dont do anything if data is not set
        if (bombInfo == null) return;

        // If the bomb is no longer hurting, dont do anything
        if (isDamageDisabled) return;

        // Get collision id
        int _id = collision.gameObject.GetInstanceID();

        // Ignore if we have already hit them
        if (collisionHistory.ContainsKey(_id)) return;
        
        // Else, try to damage them
        if (collision.gameObject.TryGetComponent<CombatEntity>(out CombatEntity other))
        {
            // Deal damage
            other.TakeDamage(bombInfo.damage, out int dmgRecieved, out Color colorSet);
            HitpointsRenderer.Instance.PrintDamage(other.transform.position, dmgRecieved, colorSet);
        }

        // Add object to list of ignores
        collisionHistory.Add(_id, null);
    }

    // Ian Fletcher: This is my previous code that used OverlapSphere, it doesnt work properly but ill leave it here
    // In case someone wants to try to fix it or something.

    /*
    // Checks for colliders within the bomb radius
    private void CheckCollisions()
    {
        // Check for Colliders on the domain
        int _id;
        float radius = this.transform.localScale.x / 2;
        int numFound = Physics.OverlapSphereNonAlloc(this.transform.position, radius, colliders);

        // Loop through collision array
        for (int i = 0; i < numFound; i++)
        {
            // Get the object id
            _id = colliders[i].gameObject.GetInstanceID();

            // If we already hit it, ignore it
            if (collisionHistory.ContainsKey(_id)) return;

            // Else, attempt to damage the object we collided against if its an entity
            if (colliders[i].gameObject.TryGetComponent<CombatEntity>(out CombatEntity other))
            {
                // Send damage to entity
                other.TakeDamage(bombInfo.damage, out int dmgRecieved, out Color colorSet);
                // Print the damage on screen
                HitpointsRenderer.Instance.PrintDamage(other.transform.position, dmgRecieved, colorSet);
            }

            // Add this collider to list of ignores
            collisionHistory.Add(_id, null);
        }
    }
    */
}
