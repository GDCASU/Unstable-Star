using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Objective : MonoBehaviour
{
	// Objects in the prefab this script gets attached to
	private TMP_Text title;
	private TMP_Text counter;
	private Slider slider;

	// Set by ObjectivePanel when this script gets attached to the prefab
	public ObjectiveData data;
	public bool complete;

	private void Start()
	{
		complete = false;
		title = transform.Find("Objective Text").GetComponent<TMP_Text>();
		counter = transform.Find("Objective Counter").GetComponent<TMP_Text>();
		slider = transform.Find("Objective Slider").GetComponent<Slider>();

		title.text = data.title;
		slider.minValue = 0;

		switch(data.type)
		{
			case ObjectiveType.KILLS:
				slider.maxValue = data.kills;
				counter.text = string.Format("{0}/{1}", (int)slider.value, (int)slider.maxValue);
				EventData.OnEnemyDeath += OnEnemyDeath;
				break;
			case ObjectiveType.KILLS_SPECIAL:
				slider.maxValue = data.kills;
				counter.text = string.Format("{0}/{1}", (int)slider.value, (int)slider.maxValue);
				EventData.OnEnemyDeath += OnEnemyDeathSpecial;
				break;
			case ObjectiveType.SURVIVE:
				// May need a hook to detect when the level starts here to start the timer
				slider.maxValue = data.time;
				counter.text = "";
				StartCoroutine(SurviveTimer());
				break;
		}
	}

	private void OnEnemyDeath(GameObject obj)
	{
		if(!complete)
		{
			slider.value++;
			counter.text = string.Format("{0}/{1}", (int)slider.value, (int)slider.maxValue);
			//Debug.Log("Enemy death " + obj.name);

			if(slider.value >= slider.maxValue)
			{
				complete = true;
				EventData.OnEnemyDeath -= OnEnemyDeath;
			}
		}
	}

	private void OnEnemyDeathSpecial(GameObject obj)
	{
		if(obj.GetComponentInChildren<Enemy>().GetStatData().enemyType != data.enemyType) return;

		if(!complete)
		{
			slider.value++;
			counter.text = string.Format("{0}/{1}", (int)slider.value, (int)slider.maxValue);
			//Debug.Log("Enemy death " + obj.name);

			if(slider.value >= slider.maxValue)
			{
				complete = true;
				EventData.OnEnemyDeath -= OnEnemyDeathSpecial;
			}
		}
	}

	private IEnumerator SurviveTimer()
	{
		while(!complete)
		{
			slider.value += Time.deltaTime;
			complete = slider.value >= slider.maxValue;
			yield return null;
		}
	}
}
