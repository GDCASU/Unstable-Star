using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class GatlingCrosshair : MonoBehaviour
{
    [SerializeField] private float speed;

    private Vector3 playerPos;
    private bool isFollowingPlayer;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = Player.Instance.gameObject.transform.position;
    }

    private void OnEnable()
    {
        // jump to player
        transform.position = playerPos;
        // follow the player
        isFollowingPlayer = true;
    }

    private void OnDisable()
    {
        // stop following player
        isFollowingPlayer = false;
    }

    private void Update()
    {
        if (isFollowingPlayer)
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
