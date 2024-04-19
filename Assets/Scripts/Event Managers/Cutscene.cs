using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Playables;

public enum CutsceneState
{
    PreLevel,
    PreBoss,
    PostBoss
}

public class Cutscene : MonoBehaviour
{
    public static Cutscene instance;
    Animator animator;

    [Header("Dialogue References")]
    [SerializeField] PlayableDirector director;
    [SerializeField] DialogueManager dialogueManager;
    [SerializeField] GameObject dialogueBox;

    [Header("Dialogue Controls")]
    CutsceneState cutsceneState = CutsceneState.PreLevel;
    [SerializeField] int startState = 0;
    [SerializeField] int numPreLevelBreaks = 0;
    [SerializeField] int numPreBossBreaks = 0;
    [SerializeField] int numPostBossBreaks = 0;
    int numBreaksCompleted = 0;

    [SerializeField] bool playOnAwake = false;
    public bool debug = false;

    [Header("Actor References")]
    [SerializeField] GameObject hud;
    [SerializeField] GameObject objectivesObject;
    [SerializeField] Boss boss;

    private void Awake()
    {
        EnsureSingleton();
        animator = GetComponent<Animator>();

        animator.enabled = false;
        director.Play();
        director.Pause();
        dialogueBox.SetActive(false);
        cutsceneState = (CutsceneState)startState;
    }

    private void Start()
    {
        if (playOnAwake) StartDialogue();
    }

    void EnsureSingleton()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if(director.state == PlayState.Playing)
        {
            if (debug) Debug.Log("Director is playing");

            UpdateState();
            // StopDialogue();
        }
    }

    void UpdateState()
    {
        if (debug) Debug.Log("Cutscene::UpdateState");
        StopDialogue();

        // Check if done with dialogue set
        numBreaksCompleted++;
        int comparator = -1;
        switch (cutsceneState)
        {
            case CutsceneState.PreLevel:
                comparator = numPreLevelBreaks;
                break;
            case CutsceneState.PreBoss:
                comparator = numPreBossBreaks;
                break;
            case CutsceneState.PostBoss:
                comparator = numPostBossBreaks;
                break;
        }

        if(numBreaksCompleted >= comparator)
        {
            // Completely finished with dialogue set
            AlertNextActor();

            numBreaksCompleted = 0;
            cutsceneState = (CutsceneState)(((int)cutsceneState + 1) % 3);
        }
        else
        {
            // Not done yet, go to next dialogue in dialogue set
            StartDialogue();
        }
    }

    void AlertNextActor()
    {
        if (debug) Debug.Log("Cutscene::AlertNextActor");
        switch (cutsceneState)
        {
            case CutsceneState.PreLevel:
                ActivateObjectives();
                break;
            case CutsceneState.PreBoss:
                ActivateBoss();
                break;
            case CutsceneState.PostBoss:
                ActivateNextScene();
                break;
        }
    }

    void ActivateObjectives()
    {
        if (debug) Debug.Log("Cutscene::ActivateObjectives");
        //objectivesObject.GetComponent<NextSceneLevel>().callNextScene();
    }

    void ActivateBoss()
    {
        if (debug) Debug.Log("Cutscene::ActivateBoss");
        boss.gameObject.SetActive(true);
        // boss.Activate
    }

    void ActivateNextScene()
    {
        if (debug) Debug.Log("Cutscene::ActivateNextScene");
        ScenesManager.instance.LoadNextScene();
    }

    #region Dialogue Set

    public void StartAnimation()
    {
        animator.enabled = true;
    }

    public void StartDialogue()
    {
        if (debug) Debug.Log("Cutscene::StartDialogue");

        hud.SetActive(false);
        dialogueBox.SetActive(true);
        dialogueManager.StartText();
        ChangeDialogue();
    }

    public void ChangeDialogue()
    {
        if (debug) Debug.Log("Cutscene::ChangeDialogue");

        dialogueManager.ChangeDialogue();
    }

    public void StopDialogue()
    {
        if (debug) Debug.Log("Cutscene::StopDialogue");

        hud.SetActive(true);
        dialogueBox.SetActive(false);
        director.Pause();
        animator.SetTrigger("DialogueDone");
    }

    #endregion
}