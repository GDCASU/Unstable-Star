using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class GatlingCrosshair : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 playerPos;
    [HideInInspector] public bool followPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = Player.Instance.gameObject.transform.position;
        transform.position = playerPos;
        followPlayer = true;
    }

    private void Update()
    {
        if (followPlayer)
            FollowPlayer();
    }

    private void FollowPlayer()
    {
        playerPos = Player.Instance.gameObject.transform.position;
        // Normalized vector between the crosshair and the player
        Vector2 direction = Vector3.Normalize(playerPos - transform.position);
        // move along that normalized vector
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }
}
