using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;

public class GatlingEnemy : Enemy
{
    /// <summary>
    /// Inheriting states from Enemy; move and shoot state
    /// </summary>
    

    private float lockOnTime = 1000f;
    private float moveVelocity = 10f;
    private bool moving = true;
    [SerializeField] private GameObject lockOn;
    public GameObject player;
    public Vector3 lockOnVelocity = Vector3.zero;
    [SerializeField] public Vector3 startPos;
    [SerializeField] public Vector3 endPos;
    [SerializeField] private ScriptableGatling gatGun;
    private bool summon = true;

    protected override void Start()
    {
        this.transform.position = startPos;
        shootComponent = this.GetComponent<ShootComponent>();
        //LockOn();
    }

    protected override void Update()
    {
        Move();
    }

    protected override void Move(){ 
        if(!summon){
            this.transform.position = endPos;
            LockOn();
            return;
        }
        Vector3 direction = -this.transform.position + endPos;
        direction = Vector3.Normalize(direction);
        this.transform.position = this.transform.position + direction * moveVelocity * Time.deltaTime;
        if(this.transform.position == endPos){
            LockOn();
            summon = false;
        }
    }

    /// <summary>
    /// Moving the lock on thingie
    /// </summary>

    private void LockOn(){ //DOESN'T WORK USE WILL BE FIXED AT SOME POINT OF MY LIFE
        float timeRem = lockOnTime;
        GameObject currentLockOn = Instantiate(lockOn, this.transform);
        while(timeRem > 0){
            lockOnVelocity = (player.transform.position - currentLockOn.transform.position) / (timeRem / 1000);
            currentLockOn.transform.position += lockOnVelocity * Time.deltaTime; 
            timeRem -= Time.deltaTime;
        }
        Shoot();
    }

    private void Shoot(){
        shootComponent.ShootWeapon(gatGun.GetWeaponObject());
    }

    protected override void WhenPlayerDies()
    {
        return;
    }

    protected override void TriggerDeath()
    {
        return;
    }
}
