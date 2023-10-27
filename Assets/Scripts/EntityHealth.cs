using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public readonly int MAX_HEALTH;
    public readonly int MAX_SHIELD;
    public readonly float SHIELD_REGEN_TIME; // in seconds
    public readonly float SHIELD_REGEN_DELAY_TIME; // in seconds
    public readonly float INVULN_TIME; // in seconds

    public int health { get; private set; }
    public int shield { get; private set; }
    public bool shieldBroken { get; private set; }
    public float shieldRegenTimer { get; private set; }
    public float shieldRegenDelayTimer { get; private set; }
    public float invulnTimer { get; private set; }

    public delegate void OnHealthChangedDelegate();
    public event OnHealthChangedDelegate OnHealthChanged;
    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;

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

                OnHealthChanged?.Invoke();
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
        OnHealthChanged?.Invoke();

        if(health <= 0)
		{
            OnDeath?.Invoke();
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
        OnHealthChanged?.Invoke();
    }
}
