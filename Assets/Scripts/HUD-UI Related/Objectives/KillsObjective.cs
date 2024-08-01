using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class KillsObjective : Objective
{
	private Slider killsSlider;

	protected override void Start()
	{
		base.Start();

		killsSlider = transform.Find("Objective Slider").GetComponent<Slider>();

		killsSlider.minValue = 0;
		killsSlider.maxValue = data.kills;
		counter.text = string.Format("{0}/{1}", (int)killsSlider.value, (int)killsSlider.maxValue);

		EventData.OnEnemyDeath += OnEnemyDeath;
	}

	private void OnEnemyDeath(GameObject obj)
	{
		if(!complete)
		{
			killsSlider.value++;
			counter.text = string.Format("{0}/{1}", (int)killsSlider.value, (int)killsSlider.maxValue);
			//Debug.Log("Enemy death " + obj.name);

			if(killsSlider.value >= killsSlider.maxValue)
			{
				complete = true;
				RaiseObjectiveComplete();
				EventData.OnEnemyDeath -= OnEnemyDeath;
			}
		}
	}
}
