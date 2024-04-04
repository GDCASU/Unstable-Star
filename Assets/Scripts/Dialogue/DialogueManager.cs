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
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("Essentials:")]
    [SerializeField] GameObject speachDialogue;
    [SerializeField] TMP_Text targetDialogue;
    [SerializeField] TMP_Text speakerText;
    [SerializeField] Image emotionSprite;

    Coroutine crt;
    string newDialogue = "";


    [Header("Timeline:")]
    [SerializeField] PlayableDirector timelinePlayer;

    bool canChange = false;
    bool start = false;


    [Header("Act Essentials:")]
    [SerializeField] DialogueOptions dialogueOptions;
    [SerializeField] GameObject dialogueObject;
    [SerializeField] string currentSceneFile;

    static string[][] currentDialogue;

    int current = 0;

    private void Start()
    {
        // initializes the current act
        currentDialogue = new string[1000][];
        currentDialogue = ReadFile(currentSceneFile, currentDialogue);
    }

    void Update()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(timelinePlayer);
        if (UnityEngine.Input.GetKeyDown(KeyCode.Return)) 
        {
            ChangeDialogue();
        }
    }

    // changes text, color, and emotion of dialogue box
    public void ChangeDialogue()
    {
        // Stops dialogue and starts timeline
        if (targetDialogue.text == newDialogue && (currentDialogue[current][0] == "BREAK" || currentDialogue[current][0] == "NOISE"))
        {
            dialogueObject.SetActive(false);
            speakerText.text = "";
            targetDialogue.text = "";
            timelinePlayer.Resume();
            start = false;
            current++;
        }

        // normal behaviors
        else if (canChange && start)
        {
            // Changes the text to the new dialogue
            newDialogue = currentDialogue[current][1];
            speachDialogue.SetActive(true);

            // Changes color, text, and emotion based on who's speaking
            Options options = dialogueOptions.CreateOptions(currentDialogue[current][0]);
            Sprite currentEmotion = dialogueOptions.GetEmotion(options, currentDialogue[current][2]);
            speakerText.color = options.color;
            speakerText.text = options.name;
            emotionSprite.sprite = currentEmotion;
            emotionSprite.color = Color.white;

            if (!options.isDialogue) // removes speaker box if there is no speaker
            {
                speachDialogue.SetActive(false);
                emotionSprite.color = new Color(0, 0, 0, 0);
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
        dialogueObject.SetActive(true);
        start = true;
        timelinePlayer.Pause();
    }

    // Takes information from text files and transfers into something the system can read
    public string[][] ReadFile(string fileName, string[][] act)
    {
        int dialogueIndex = 0;
        string currentSpeaker = "";
        string emotion = "NORMAL";

        foreach (var line in System.IO.File.ReadLines(fileName))
        {
            if (line == "END") // Checks if the file is done
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
                || line == "SECURITY DEFENSE SYSTEM" || line == "PRISON WARDEN" || line == "Radio Station DJ" 
                || line == "JOHN" || line == "DJ Treble Make-R" || line == "SHADOWY FIGURE"
                || line == "JAUGHN’S SUPERVISOR" || line == "Random Civilian" || line == "Mathematically Predictable Civilian"
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
                // Debug.Log(currentSpeaker + ": " + line);
            }
        }
        return act;
    }

    // Changes to the next scene
    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }
}
