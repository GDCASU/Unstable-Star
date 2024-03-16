using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Essentials:")]
    [SerializeField] Sprite noSpeachDialogue;
    [SerializeField] Sprite speachDialogue;
    [SerializeField] Image speachDialogueImage;
    [SerializeField] TMP_Text targetDialogue;
    [SerializeField] TMP_Text speakerText;

    Coroutine crt;
    string newDialogue = "";


    [Header("Timeline:")]
    [SerializeField] PlayableDirector timelinePlayer;

    bool canChange = false;
    bool start = false;

    [Header("Scene Essentials:")]
    int actNumber = 1;
    int sceneNumber = 1;
    int current;

    string[] currentDialogue;
    string[] currentSpeakers;

    [Header("Act One Scene One:")]
    [SerializeField] string[] actOneDialogue;
    [SerializeField] string[] actOneSpeakers;

    [Header("Act One Scene Two:")]
    [SerializeField] string[] actOne_TwoDialogue;
    [SerializeField] string[] actOne_TwoSpeakers;

    [Header("Act One Scene Three:")]
    [SerializeField] string[] actOne_ThreeDialogue;
    [SerializeField] string[] actOne_ThreeSpeakers;

    [Header("Act Two Scene One:")]
    [SerializeField] string[] actTwo_OneDialogue;
    [SerializeField] string[] actTwo_OneSpeakers;

    [Header("Act Two Scene Two:")]
    [SerializeField] string[] actTwo_TwoDialogue;
    [SerializeField] string[] actTwo_TwoSpeakers;

    [Header("Act Two Scene Three:")]
    [SerializeField] string[] actTwo_ThreeDialogue;
    [SerializeField] string[] actTwo_ThreeSpeakers;

    [Header("Act Three Scene One:")]
    [SerializeField] string[] actThree_OneDialogue;
    [SerializeField] string[] actThree_OneSpeakers;

    [Header("Act Three Scene Two:")]
    [SerializeField] string[] actThree_TwoDialogue;
    [SerializeField] string[] actThree_TwoSpeakers;

    [Header("Act Three Scene Three:")]
    [SerializeField] string[] actThree_ThreeDialogue;
    [SerializeField] string[] actThree_ThreeSpeakers;

    [Header("Act Three Scene Four:")]
    [SerializeField] string[] actThree_FourDialogue;
    [SerializeField] string[] actThree_FourSpeakers;

    [Header("Act Three Scene Five:")]
    [SerializeField] string[] actThree_FiveDialogue;
    [SerializeField] string[] actThree_FiveSpeakers;


    private void Start()
    {
        // initializes the acts
        InitializeActs();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && start) 
        {
            ChangeDialogue();
        }
    }

    public void ChangeDialogue()
    {
        SetCurrentScene();

        // Stops dialogue and starts scene
        if (currentSpeakers[current] == "Break")
        {
            timelinePlayer.Resume();
            start = false;
            current++;
        }

        if (canChange && start) 
        {
            // Changes the text to the new dialogue
            newDialogue = currentDialogue[current];
            speakerText.text = currentSpeakers[current];
            speachDialogueImage.sprite = speachDialogue;

            // Changes color based on who's speaking

            if (currentSpeakers[current] == "Rebekah")
            {
                targetDialogue.color = Color.red;
                speakerText.color = Color.red;
            }
            else if (currentSpeakers[current] == "Jaughn")
            {
                targetDialogue.color = Color.grey;
                speakerText.color = Color.grey;
            }
            else if (currentSpeakers[current] == "Apollo")
            {
                targetDialogue.color = Color.blue;
                speakerText.color = Color.blue;
            }
            else if (currentSpeakers[current] == "Ebb")
            {
                targetDialogue.color = Color.yellow;
                speakerText.color = Color.yellow;
            }
            else if (currentSpeakers[current] == "Noise")
            {
                targetDialogue.color = Color.black;
                speachDialogueImage.sprite = noSpeachDialogue;
                speakerText.text = "";
            }
            else
            {
                targetDialogue.color = Color.black;
                speakerText.color = Color.black;
            }

            // Finishing touches
            current++;
            canChange = false;

            // Starts the coroutine that makes the text typewriter-y
            if (targetDialogue.text != newDialogue)
            {
                if (crt != null)
                    StopCoroutine(crt);
                crt = StartCoroutine(setTooltipText(newDialogue));
            }
        }

        else if (start)
        {
            targetDialogue.text = newDialogue;
            canChange = true;
            if (crt != null)
                StopCoroutine(crt);
        }
    }

    public void SetCurrentScene()
    {
        switch (actNumber)
        {
            case 1: 
                switch(sceneNumber)
                {
                    case 1:
                        currentDialogue = actOneDialogue;
                        currentSpeakers = actOneSpeakers;
                        break;
                    case 2:
                        currentDialogue = actOne_TwoDialogue;
                        currentSpeakers = actOne_TwoSpeakers;
                        break;
                    case 3:
                        currentDialogue = actOne_ThreeDialogue;
                        currentSpeakers = actOne_ThreeSpeakers;
                        break;
                }
                break;
            case 2:
                switch (sceneNumber)
                {
                    case 1:
                        currentDialogue = actTwo_OneDialogue;
                        currentSpeakers = actTwo_OneSpeakers;
                        break;
                    case 2:
                        currentDialogue = actTwo_TwoDialogue;
                        currentSpeakers = actTwo_TwoSpeakers;
                        break;
                    case 3:
                        currentDialogue = actTwo_ThreeDialogue;
                        currentSpeakers = actTwo_ThreeSpeakers;
                        break;
                }
                break;
            case 3:
                switch (sceneNumber)
                {
                    case 1:
                        currentDialogue = actThree_OneDialogue;
                        currentSpeakers = actThree_OneSpeakers;
                        break;
                    case 2:
                        currentDialogue = actThree_TwoDialogue;
                        currentSpeakers = actThree_TwoSpeakers;
                        break;
                    case 3:
                        currentDialogue = actThree_ThreeDialogue;
                        currentSpeakers = actThree_ThreeSpeakers;
                        break;
                    case 4:
                        currentDialogue = actThree_FourDialogue;
                        currentSpeakers = actThree_FourSpeakers;
                        break;
                    case 5:
                        currentDialogue = actThree_FiveDialogue;
                        currentSpeakers = actThree_FiveSpeakers;
                        break;
                }
                break;
        }
    }

    IEnumerator setTooltipText(string str)
    {
        targetDialogue.text = "";
        while (newDialogue != targetDialogue.text)
        {
            targetDialogue.text = str.Substring(0, targetDialogue.text.Length + 1);
            yield return new WaitForSeconds(0.05f);
        }
        canChange = true;
    }

    public void PauseTimeline()
    {
        timelinePlayer.Pause();
    }

    public void StartText()
    {
        start = true;
        PauseTimeline();
    }

    public void SceneSwitch()
    {
        sceneNumber++;
        switch (actNumber)
        {
            case 1:
            case 2:
                if (sceneNumber > 3)
                {
                    sceneNumber = 0;
                    actNumber++;
                }
                break;
            case 3:
                if (sceneNumber > 5)
                {
                    sceneNumber = 0;
                    actNumber++;
                }
                break;
        }
    }

    public void InitializeActs()
    {
        int speakerIndex = 0;
        int dialogueIndex = 0;
        foreach (var line in File.ReadLines("file"))
        {
            if (line == "APOLLO")
            {

            }
            if (line == "REBEKAH")
            {

            }
            if (line == "EBB")
            {

            }
            if (line == "JAUGHN")
            {

            }
            dialogueIndex++;
            speakerIndex++;
        }
    }
}
