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

    //Will be used to detect entity collision with other Entities
    // (AKA: Asteroids and enemies) (not players)
    private void OnTriggerEnter(Collider other)
    {
        parentScript.TakeCollisionDamage(other);
    }

    //IDamageable reference on parent, needed by the player to perform an entity collision
    public void TakeDamage(int damage, out int dmgRecieved, out bool wasShield)
    {
        parentScript.TakeDamage(damage, out int dmgTaken, out bool shieldEval);
        dmgRecieved = dmgTaken;
        wasShield = shieldEval;
    }
}
