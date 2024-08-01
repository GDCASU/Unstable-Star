using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public abstract class Objective : MonoBehaviour
{
	// Objects in the prefab this script gets attached to
	protected TMP_Text title;
	protected TMP_Text counter;

	// Set by ObjectivePanel when this script gets attached to the prefab
	public ObjectiveData data;
	protected bool complete;

	public event System.Action<Objective> OnObjectiveComplete;
	public void RaiseObjectiveComplete()
	{
		OnObjectiveComplete?.Invoke(this);
	}

	protected virtual void Start()
	{
		complete = false;
		title = transform.Find("Objective Text").GetComponent<TMP_Text>();
		counter = transform.Find("Objective Counter").GetComponent<TMP_Text>();
		title.text = data.title;
	}
}
