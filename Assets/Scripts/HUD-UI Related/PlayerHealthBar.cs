using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
	[SerializeField] private Player player;
	private Transform healthBarEmpty;
	private Transform healthBar;
	private Transform shieldBarEmpty;
	private Transform shieldBar;

	private void Start()
	{
		// Subscribe to Events
		EventData.OnHealthAdded += OnHealthChanged;
		EventData.OnHealthLost += OnHealthChanged;


        healthBarEmpty = transform.Find("Health Bar Empty");
		healthBar = transform.Find("Health Bar");
		shieldBarEmpty = transform.Find("Shield Bar Empty");
		shieldBar = transform.Find("Shield Bar");

		// Scale the UI segments to whatever the max health/shield is
		if(player.GetMaxHealth() > 1)
		{
			foreach(Transform t in new Transform[] { healthBarEmpty, healthBar })
			{
				for(int i = 2; i < player.GetMaxHealth(); i++)
				{
					GameObject segment = Instantiate(t.GetChild(i - 1).gameObject, t);
					segment.transform.localPosition = t.GetChild(i - 1).localPosition + new Vector3(22, 0, 0);
				}
			}
		}
		else
		{
			Destroy(healthBarEmpty.Find("Middle").gameObject);
			Destroy(healthBar.Find("Middle").gameObject);
		}
		if(player.GetMaxShield() > 1)
		{
			foreach(Transform t in new Transform[]{shieldBarEmpty, shieldBar})
			{
				for(int i = 2; i < player.GetMaxShield(); i++)
				{
					GameObject segment = Instantiate(t.GetChild(i - 1).gameObject, t);
					segment.transform.localPosition = t.GetChild(i - 1).localPosition + new Vector3(22, 0, 0);
				}
			}
		}
		else
		{
			Destroy(shieldBarEmpty.Find("Middle").gameObject);
			Destroy(shieldBar.Find("Middle").gameObject);
		}
		
		// Testing code
		//player.TakeDamage(5, out _, out _);
		//OnHealthChanged();
	}

	private void OnHealthChanged(int currentHealth)
	{
		for(int i = 0; i < healthBar.childCount; i++)
		{
			healthBar.GetChild(i).gameObject.SetActive(i < player.GetHealth());
		}
		for(int i = 0; i < shieldBar.childCount; i++)
		{
			shieldBar.GetChild(i).gameObject.SetActive(i < player.GetShield());
		}
	}
}
