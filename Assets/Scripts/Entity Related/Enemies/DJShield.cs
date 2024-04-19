using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DJShield : CombatEntity
{
	public event System.Action<GameObject> OnShieldDestroyed;

	protected override void TriggerDeath()
	{
		OnShieldDestroyed?.Invoke(gameObject);
		Destroy(gameObject);
	}

	protected override void WhenPlayerDies()
	{
		throw new System.NotImplementedException();
	}
}
