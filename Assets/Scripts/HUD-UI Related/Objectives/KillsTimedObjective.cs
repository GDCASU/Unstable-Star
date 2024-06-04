using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class KillsTimedObjective : Objective
{
	private Slider killsSlider;
	private Slider timerSlider;

	protected override void Start()
	{
		base.Start();

		killsSlider = transform.Find("Objective Slider").GetComponent<Slider>();
		timerSlider = transform.Find("Timer Slider").GetComponent<Slider>();

		killsSlider.minValue = 0;
		killsSlider.maxValue = data.kills;
		timerSlider.minValue = 0;
		timerSlider.maxValue = data.time;
		counter.text = string.Format("{0}/{1}", (int)killsSlider.value, (int)killsSlider.maxValue);

		EventData.OnEnemyDeath += OnEnemyDeath;
		StartCoroutine(KillsTimer());
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

	private IEnumerator KillsTimer()
	{
		while(!complete)
		{
			timerSlider.value += Time.deltaTime;
			if(timerSlider.value >= timerSlider.maxValue && killsSlider.value < data.kills)
			{
				// Game over
			}
			yield return null;
		}
		timerSlider.value = timerSlider.maxValue;
	}
}
