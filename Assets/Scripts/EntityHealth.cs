using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] private readonly int MAX_HEALTH;
    [SerializeField] private readonly int MAX_SHIELD;
    [SerializeField] private readonly float SHIELD_REGEN_TIME; // in seconds
    [SerializeField] private readonly float SHIELD_REGEN_DELAY_TIME; // in seconds
    [SerializeField] private readonly float INVULN_TIME; // in seconds

    public int health { get; private set; }
    public int shield { get; private set; }
    public bool shieldBroken { get; private set; }
    public float shieldRegenTimer { get; private set; }
    public float shieldRegenDelayTimer { get; private set; }
    public float invulnTimer { get; private set; }

    // Need health changed, death, and maybe shield broken events

	private void Start()
	{
        health = MAX_HEALTH;
        shield = MAX_SHIELD;
	}

	private void Update()
	{
        invulnTimer -= Time.deltaTime;

        // For entities that don't have a shield
        if(MAX_SHIELD <= 0) return;

        // Check for shield regen delay
        shieldRegenDelayTimer -= Time.deltaTime;
        if(shield < MAX_SHIELD && shieldRegenDelayTimer <= 0f)
		{
            shieldRegenTimer -= Time.deltaTime;
            if(shieldRegenTimer <= 0f)
            {
                shield++;
                if(shield > MAX_SHIELD)
                {
                    shield = MAX_SHIELD;
                }
                shieldRegenTimer = SHIELD_REGEN_TIME;

                // Raise health changed event
            }
        }
    }

	public void TakeDamage(int damage)
	{
        if(invulnTimer > 0f) return;

        if(shieldBroken)
		{
            health -= damage;
		}
		else
		{
            int remainder = damage - shield;
            if(remainder > 0)
            {
                shield = 0;
                health -= remainder;
            }
            else
            {
                shield -= damage;
            }

            if(shield <= 0)
			{
                shieldBroken = true;
                shieldRegenTimer = SHIELD_REGEN_TIME;
                shieldRegenDelayTimer = SHIELD_REGEN_DELAY_TIME;

                // Raise shield broken event?
            }
        }

        // Raise health changed event

        if(health <= 0)
		{
            // Raise death event
		}

        invulnTimer = INVULN_TIME;
    }

    public void AddHealth(int amount)
	{
        health += amount;
        if(health > MAX_HEALTH)
		{
            health = MAX_HEALTH;
		}
        // Raise health changed event
	}
}
