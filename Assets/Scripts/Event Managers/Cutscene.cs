using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    Animator animator;

    public List<GameObject> dialogue = new List<GameObject>();
    int currentDia = -1;

    bool inDialogue = false;
    public bool debug = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        foreach(var dia in dialogue) dia.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) ProgressDialogue();
    }

    /// <summary>
    /// Starts dialogue, allowing the player to click the attack button to progress.
    /// </summary>
    public void StartDialogue()
    {
        if (debug) Debug.Log("Cutscene::StartDialogue");
        inDialogue = true;
        ProgressDialogue();
    }

    public void ProgressDialogue()
    {
        if (debug) Debug.Log("Cutscene::ProgressDialogue");
        if (!inDialogue) return;

        if(++currentDia >=  dialogue.Count)
        {
            EndDialogue();
            return;
        }

        if (currentDia > 0) dialogue[currentDia - 1].SetActive(false);
        dialogue[currentDia].SetActive(true);
    }

    /// <summary>
    /// Ends the current dialogue and allows the animator to progress.
    /// </summary>
    public void EndDialogue()
    {
        if (debug) Debug.Log("Cutscene::EndDialogue");

        inDialogue = false;
        foreach (var dia in dialogue) dia.SetActive(false);
        currentDia = -1;
        animator.SetTrigger("DialogueDone");
    }
}
