using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
	[SerializeField] private TMP_Text title;
	[SerializeField] private TMP_Text counter;
	[SerializeField] private Slider slider;

	public void SetupData(ObjectiveData objectiveData)
	{
		title.text = objectiveData.title;
		slider.minValue = objectiveData.minValue;
		slider.maxValue = objectiveData.maxValue;
		counter.text = string.Format("{0}/{1}", (int)slider.value, (int)slider.maxValue);

		// Right now, the only objective is a kill x enemies objective
		// To add more objective types, add a new objective type to the enum in ObjectivePanel and then add functionality here
		switch(objectiveData.type)
		{
			case ObjectiveType.KILLS:
				EventData.OnEnemyDeath += OnEnemyKilled;
				break;
		}
	}

	private void OnEnemyKilled(GameObject nop)
	{
		if(slider.value < slider.maxValue)
		{
			slider.value++;
			counter.text = string.Format("{0}/{1}", (int)slider.value, (int)slider.maxValue);
		}
	}
}
