using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Playables;

public class Cutscene : MonoBehaviour
{
    Animator animator;
    [SerializeField] PlayableDirector director;
    [SerializeField] DialogueManager dialogueManager;

    [SerializeField] bool playOnAwake = false;
    public bool debug = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        animator.enabled = playOnAwake;
        director.Play();
        director.Pause();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (debug) Debug.Log("Space pressed.");
        }

        if(director.state == PlayState.Playing)
        {
            if (debug) Debug.Log("Director is playing");

            StopDialogue();
        }
    }

    public void StartAnimation()
    {
        animator.enabled = true;
    }

    public void StartDialogue()
    {
        if (debug) Debug.Log("Cutscene::StartDialogue");

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

        director.Pause();
        animator.SetTrigger("DialogueDone");
    }
}
