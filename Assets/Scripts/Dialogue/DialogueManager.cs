using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;

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

    [Header("Debugging:")]
    [SerializeField] private bool doDebugLog;

    static string[][] currentDialogue;

    int current = 0;

    private void Start()
    {
        // initializes the current act
        currentSceneFile = InputChecker();
        currentDialogue = new string[1000][];
        currentDialogue = ReadFile(System.IO.Path.Combine(Application.streamingAssetsPath, currentSceneFile), currentDialogue);

        dialogueOptions ??= GetComponent<DialogueOptions>();
        if (dialogueOptions == null) throw new NullReferenceException("Dialogue options not initialized.");

        PlayerInput.OnChangeDialogue += ChangeDialogue; // Change dialogue inputs from the input system
        PlayerInput.OnSkipDialogue += ChangeScene;  // Change Scene inputs
    }

    private void OnDestroy()
    {
        PlayerInput.OnChangeDialogue -= ChangeDialogue;
        PlayerInput.OnSkipDialogue -= ChangeScene;
    }

    // changes text, color, and emotion of dialogue box
    public void ChangeDialogue()
    {
        if (speachDialogue == null) return;

        // Stops dialogue and starts timeline
        if (canChange && (currentDialogue[current] == null || currentDialogue[current][0] == "BREAK" || currentDialogue[current][0] == "NOISE" || currentDialogue[current][0] == "END"))
        {
            if (doDebugLog) Debug.Log("DialogueManager::ChangeDialogue::End");

            StopText();
        }   

        // normal behaviors
        else if (canChange && start)
        {
            if (doDebugLog) Debug.Log("DialogueManager::ChangeDialogue::ChangeAndStart");

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
                emotionSprite.color = new Color(0, 0, 0, 0);
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

        // stops the couroutine and autofills the text
        else if (start) 
        {
            if (doDebugLog) Debug.Log("DialogueManager::ChangeDialogue::Start");
            targetDialogue.text = newDialogue;
            canChange = true;
            if (crt != null)
                StopCoroutine(crt);
        }

        if (doDebugLog) Debug.Log("DialogueManager::ChangeDialogue");
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
        canChange = true;
    }

    // Pauses timeline and makes the text clickable
    public void StartText()
    {
        dialogueObject.SetActive(true);
        start = true;
        timelinePlayer.Pause();
        ChangeDialogue();
        ChangeDialogue();
    }

    // Starts timeline and makes the text stop
    public void StopText()
    {
        if (speachDialogue == null) { return; }

        // removes the emotion sprite
        speachDialogue.SetActive(false);
        emotionSprite.color = new Color(0, 0, 0, 0);

        // makes the dialogue box empty
        dialogueObject.SetActive(false);
        speakerText.text = "";
        targetDialogue.text = "";

        // pauses the dialogue and runs the animations
        timelinePlayer.Resume();
        start = false;
        current++;
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

            // If line is a name
            else if (line == "APOLLO" || line == "REBEKAH" || line == "EBB" || line == "JAUGHN" 
                || line == "SECURITY DEFENSE SYSTEM" || line == "PRISON WARDEN" || line == "Radio Station DJ" 
                || line == "JOHN" || line == "DJ Treble Make-R" || line == "SHADOWY FIGURE"
                || line == "JAUGHN’S SUPERVISOR" || line == "COALITION GRUNT" || line == "Random Civilian" || line == "Mathematically Predictable Civilian"
                || line == "PIZZA DELIVERY DRIVER" || line == "DESC: " || line == "NOISE" || line == "BREAK") 
            { 
                currentSpeaker = line;
            }

            // If line is an emotion
            else if (line == "HAPPY" || line == "NORMAL" || line == "ANGRY" || line == "SAD" || line == "???") 
            {
                emotion = line;
            }

            // If line isn't blank, store dialogue
            else if (!string.IsNullOrWhiteSpace(line)) 
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

    // Changes to the next scene
    public void ChangeScene()
    {
        for (int i=1; i< 8; i++)
        {
            if (SceneManager.GetActiveScene().name == "CutScene_" + i.ToString())
            {
                ScenesManager.instance.LoadNextScene();
                // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
            }
        }
    }

    // Checks to see what the current input is
    private string InputChecker() {
        Gamepad gamepad = Gamepad.current;
        Keyboard keyboard = Keyboard.current;

        if (gamepad != null)
        {
            if (gamepad is XInputController)
            {
                // Player is using an XBOX controller
                // Updates the tutorial
                if (currentSceneFile == "Act1_Sc2.txt")
                {
                    Debug.Log("Controller");
                    return "Act1_Sc2 Controller.txt";
                }
            }
        }
        else if (keyboard != null)
        {
            // Player is using a keyboard
            // Updates the tutorial
            if (currentSceneFile == "Act1_Sc2.txt")
            {
                Debug.Log("Keyboard");
                return "Act1_Sc2 Keyboard.txt";
            }
        }

        return currentSceneFile;
    }
}
