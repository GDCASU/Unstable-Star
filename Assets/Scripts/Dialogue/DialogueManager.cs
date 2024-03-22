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
using UnityEngine.Windows;
using OpenCover.Framework.Model;
using UnityEditor.ShaderGraph;

public class DialogueManager : MonoBehaviour
{
    [Header("Essentials:")]
    [SerializeField] Sprite noSpeachDialogue;
    [SerializeField] Sprite speachDialogue;
    [SerializeField] Image speachDialogueImage;
    [SerializeField] TMP_Text targetDialogue;
    [SerializeField] TMP_Text speakerText;
    [SerializeField] Image emotionSprite;

    Coroutine crt;
    string newDialogue = "";


    [Header("Timeline:")]
    [SerializeField] PlayableDirector timelinePlayer;

    bool canChange = false;
    bool start = false;

    [Header("Scene Essentials:")]
    [SerializeField] DialogueOptions dialogueOptions;

    string[][] currentDialogue;

    string[][] actOne = new string[1000][];
    string[][] actTwo = new string[1000][];
    string[][] actThree = new string[1000][];

    int actNumber = 1;
    int current = 0;

    

    private void Start()
    {
        // initializes the acts
        actOne = ReadFile("Assets/Scripts/Dialogue/Act_One.txt", actOne, "One");
        actTwo = ReadFile("Assets/Scripts/Dialogue/Act_Two.txt", actTwo, "Two");
        actThree = ReadFile("Assets/Scripts/Dialogue/Act_Three.txt", actThree, "Three");
    }

    void Update()
    {
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return)) 
        {
            ChangeDialogue();
        }
    }

    // changes text, color, and emotion of dialogue box
    public void ChangeDialogue()
    {
        SetCurrentAct();

        // Stops dialogue and starts scene
        if (currentDialogue[current][0] == "BREAK" || currentDialogue[current][0] == "NOISE")
        {
            speachDialogueImage.color = Color.clear;
            timelinePlayer.Resume();
            start = false;
            current++;
        }
        
        // changes act if the script has ended
        if (currentDialogue[current][0] == "END")
        {
            actNumber++;
            SetCurrentAct();
        }

        // normal behaviors
        else if (canChange && start)
        {
            // Changes the text to the new dialogue
            newDialogue = currentDialogue[current][1];
            speachDialogueImage.sprite = speachDialogue;

            // Changes color, text, and emotion based on who's speaking
            Options options = dialogueOptions.CreateOptions(currentDialogue[current][0]);
            Sprite currentEmotion = dialogueOptions.GetEmotion(options, currentDialogue[current][2]);
            speakerText.color = options.color;
            speakerText.text = options.name;

            if (!options.isDialogue) // removes speaker box if there is no speaker
            {
                speachDialogueImage.sprite = noSpeachDialogue;
            }

            if (currentDialogue[current][2] == "???") // Creates question marks for unknown people
            {
                speakerText.text = "???";
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
                //canChange = true;
            }
        }

        // stops the couroutine and autofills the text
        else if (start) 
        {
            targetDialogue.text = newDialogue;
            canChange = true;
            if (crt != null)
                StopCoroutine(crt);
        }
    }

    // sets the act based on what the act number is
    public void SetCurrentAct()
    {
        switch (actNumber)
        {
            case 1:
                currentDialogue = actOne;
                break;
            case 2:
                currentDialogue = actTwo;
                break;
            case 3:
                currentDialogue = actThree;
                break;
        }
    }

    // Creates text that appears like a typewriter
    IEnumerator setTooltipText(string str)
    {
        targetDialogue.text = "";
        while (newDialogue != targetDialogue.text)
        {
            targetDialogue.text = str.Substring(0, targetDialogue.text.Length + 1);
            yield return new WaitForSeconds(0.05f);
        }
    }

    // Pauses timeline and makes the text clickable
    public void StartText()
    {
        speachDialogueImage.color = Color.white;
        start = true;
        timelinePlayer.Pause();
    }

    // Takes information from text files and transfers into something the system can read
    public string[][] ReadFile(string fileName, string[][] act, string number)
    {
        int dialogueIndex = 0;
        string currentSpeaker = "";
        string emotion = "NORMAL";

        foreach (var line in System.IO.File.ReadLines(fileName))
        {
            if (line == "End of Act " + number) // Checks if the file is done
            {
                act[dialogueIndex] = new string[3];
                act[dialogueIndex][0] = "END";
                act[dialogueIndex][1] = "END";
                act[dialogueIndex][2] = "END";
                return act;
            }

            else if (line == "Arcade Game Script") // Creates a break
            {
                act[dialogueIndex] = new string[3];
                act[dialogueIndex][0] = "BREAK";
                act[dialogueIndex][1] = "BREAK";
                act[dialogueIndex][2] = "BREAK";
            }

            // If line is a name
            else if (line == "APOLLO" || line == "REBEKAH" || line == "EBB" || line == "JAUGHN" 
                || line == "SECURITY DEFENSE SYSTEM" || line == "PRISON WARDEN" || line == "JAUGHN’S SUPERVISOR"
                || line == "PIZZA DELIVERY DRIVER" || line == "DESC: " || line == "NOISE") 
            { 
                currentSpeaker = line;
            }

            // if line is an emotion
            else if (line == "HAPPY" || line == "NORMAL" || line == "ANGRY" || line == "SAD" || line == "???") 
            {
                emotion = line;
            }

            // if line isn't blank, store dialogue
            else if (line != "") 
            {
                act[dialogueIndex] = new string[3];
                act[dialogueIndex][0] = currentSpeaker;
                act[dialogueIndex][1] = line;
                act[dialogueIndex][2] = emotion;
                dialogueIndex++;
                Debug.Log(currentSpeaker + ": " + line);
            }
        }
        return act;
    }
}
