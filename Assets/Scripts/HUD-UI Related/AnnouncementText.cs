using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnnouncementText : MonoBehaviour
{
	public static AnnouncementText Instance;
	private TMP_Text announcementText;

	private void Awake()
	{
		if(Instance == null)
		{
			Instance = this;
			return;
		}
		Destroy(this);
	}

	private void Start()
	{
		announcementText = GetComponent<TMP_Text>();
		SetText("", 0f, false);
	}

	// Testing Code
	//private void Update()
	//{
	//	if(Input.GetKeyDown(KeyCode.I))
	//	{
	//		SetText("This is an announcement", 3f, false);
	//	}
	//	if(Input.GetKeyDown(KeyCode.O))
	//	{
	//		SetText("This is an announcement", 3f, true);
	//	}
	//}

	/// <summary>
	/// Sets the announement text. If immediate, the text is displayed immediately. Otherwise, the text scrolls one character at a time.
	/// </summary>
	public void SetText(string text, float time, bool immediate)
	{
		StopAllCoroutines();
		StartCoroutine(SetTextInternal(text, time, immediate));
	}

	private IEnumerator SetTextInternal(string text, float time, bool immediate)
	{
		announcementText.enabled = true;
		announcementText.text = "";
		for(int i = 0; i < text.Length; i++)
		{
			announcementText.text += text[i].ToString();
			if(!immediate) yield return new WaitForSeconds(0.075f);
		}
		yield return new WaitForSeconds(time);
		announcementText.text = "";
		announcementText.enabled = false;
	}
}
