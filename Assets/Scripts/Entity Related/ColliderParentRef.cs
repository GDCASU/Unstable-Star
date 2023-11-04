using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Helps with the colliding among entities that arent the player, nothing else
public class ColliderParentRef : MonoBehaviour, IDamageable
{
    private GameObject parentObject;
    private CombatEntity parentScript;

    void Awake()
    {
        parentObject = this.gameObject.transform.parent.gameObject;
        parentScript = parentObject.GetComponent<CombatEntity>();
    }

    //Will be used to detect entity collision with other Entities (except player)
    private void OnTriggerEnter(Collider other)
    {
        parentScript.TakeCollisionDamage(other);
    }

    //IDamageable reference on parent, needed by the player collider
    public void TakeDamage(int damage)
    {
        parentScript.TakeDamage(damage);
    }
}
