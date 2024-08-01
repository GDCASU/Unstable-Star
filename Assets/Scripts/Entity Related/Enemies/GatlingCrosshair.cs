using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class GatlingCrosshair : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Animator animator;
    [SerializeField] private FMODUnity.EventReference lockOnSFX;
    [SerializeField] private FMODUnity.StudioEventEmitter searchingSFXEmitter;
    [HideInInspector] public bool canFollowPlayer;
    [HideInInspector] public bool followPlayer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CrosshairFollow());
    }

    /// <summary>
    /// Follows the player and interacts with the animator
    /// </summary>
    /// <returns></returns>
    private IEnumerator CrosshairFollow()
    {
        // First wait for the gatling enemy parent script to tell us to start
        while (!canFollowPlayer) yield return null;

        // Start shooting loop till death
        while (true)
        {
            // By default following player, so play follow sound before waiting
            searchingSFXEmitter.Play();
            animator.SetBool("isFiring", false);

            while (followPlayer)
            {
                FollowPlayer();
                yield return null;
            }

            // Crosshair stoped
            searchingSFXEmitter.Stop();
            SoundManager.instance.PlaySound(lockOnSFX);
            animator.SetBool("isFiring", true);

            while (!followPlayer) yield return null;
        }
    }

    private void FollowPlayer()
    {
        // Dont run if player is not present
        if (Player.instance == null) return;
        // Else, run
        Vector3 playerPos = Player.instance.gameObject.transform.position;
        // Normalized vector between the crosshair and the player
        Vector2 direction = Vector3.Normalize(playerPos - transform.position);
        // move along that normalized vector
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
