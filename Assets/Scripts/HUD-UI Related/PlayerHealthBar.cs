using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBar : MonoBehaviour
{
	private Transform healthBarEmpty;
	private Transform healthBar;
	private Transform shieldBarEmpty;
	private Transform shieldBar;

	private void Start()
	{
		healthBarEmpty = transform.Find("Health Bar Empty");
		healthBar = transform.Find("Health Bar");
		shieldBarEmpty = transform.Find("Shield Bar Empty");
		shieldBar = transform.Find("Shield Bar");

		// Scale the UI segments to whatever the max health/shield is
		if(Player.Instance.GetMaxHealth() > 1)
		{
			foreach(Transform t in new Transform[] { healthBarEmpty, healthBar })
			{
				for(int i = 2; i < Player.Instance.GetMaxHealth(); i++)
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
		if(Player.Instance.GetMaxShield() > 1)
		{
			foreach(Transform t in new Transform[]{shieldBarEmpty, shieldBar})
			{
				for(int i = 2; i < Player.Instance.GetMaxShield(); i++)
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

	private void OnHealthChanged()
	{
		for(int i = 0; i < healthBar.childCount; i++)
		{
			healthBar.GetChild(i).gameObject.SetActive(i < Player.Instance.GetHealth());
		}
		for(int i = 0; i < shieldBar.childCount; i++)
		{
			shieldBar.GetChild(i).gameObject.SetActive(i < Player.Instance.GetShield());
		}
	}
}
