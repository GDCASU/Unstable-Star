using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SurviveObjective : Objective
{
	private Slider timerSlider;

	protected override void Start()
	{
		base.Start();

		timerSlider = transform.Find("Objective Slider").GetComponent<Slider>();

		timerSlider.minValue = 0;
		timerSlider.maxValue = data.time;
		counter.text = "";

		StartCoroutine(SurviveTimer());
	}

	private IEnumerator SurviveTimer()
	{
		while(!complete)
		{
			timerSlider.value += Time.deltaTime;
			complete = timerSlider.value >= timerSlider.maxValue;
			yield return null;
		}
		RaiseObjectiveComplete();
	}
}
