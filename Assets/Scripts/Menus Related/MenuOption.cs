using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuOption : MonoBehaviour // changes behaviours of options part of menu
{
    public string optionName;
    public bool subMenu;
    public Vector3 cameraPos;
    public Vector3 cameraRotation;
    private Vector3 defaultCameraPos;
    private Vector3 defaultCameraRotation;
    public float moveSpeed;
    public float rotationSpeed;
    private bool inSubmenu;

    public AudioClip myAudioClip;
    private AudioSource audioSource;

    private Coroutine moveCoroutine;
    private Coroutine rotationCoroutine;

    private Quaternion defaultRotation;

    private tooltipText ttText;

    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = myAudioClip;

        defaultCameraPos = Camera.main.transform.position;
        defaultCameraRotation = Camera.main.transform.rotation.eulerAngles;

        defaultRotation = transform.rotation;

        ttText = GameObject.Find("tooltipText").GetComponent<tooltipText>();

        inSubmenu = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && inSubmenu) // Detect right click 
        {
            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            //if (rotationCoroutine != null)
            //StopCoroutine(rotationCoroutine);
            ttText.newText = "Welcome";
            moveCoroutine = StartCoroutine(MoveCamera(defaultCameraPos, defaultCameraRotation, false));
            //moveCoroutine = StartCoroutine(moveCameraPos(MenuManager.defaultCameraPosition));
            //rotationCoroutine = StartCoroutine(moveCameraRotBack());
        }

    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (optionName == "Credits") ScenesManager.instance.LoadScene(Scenes.Credits);

            if (moveCoroutine != null)
                StopCoroutine(moveCoroutine);
            moveCoroutine = StartCoroutine(MoveCamera(cameraPos, cameraRotation, true));
            inSubmenu = true;
        }
    }
    
    void OnMouseEnter()
    {
        audioSource.Play();
        if (!inSubmenu)
        {
            transform.Rotate(new Vector3(0, 10, 0));
            ttText.newText = optionName;
        }
    }

    void OnMouseExit()
    {
        if (!inSubmenu)
        {
            transform.rotation = defaultRotation;
            //transform.Rotate(new Vector3(0, -10, 0));
            ttText.newText = "Welcome";
        }
            
    }

    IEnumerator MoveCamera(Vector3 targetPosition, Vector3 targetRotationEulerAngles, bool newInSub)
    {
        float startTime = Time.time;

        Vector3 initialPosition = Camera.main.transform.position;
        Quaternion initialRotation = Camera.main.transform.rotation;

        // Debug.Log("INIT: " + initialPosition + "  " + initialRotation.eulerAngles);
        // Debug.Log("TARGET: " + targetPosition + "  " + targetRotationEulerAngles);

        float journeyLengthPos = Vector3.Distance(initialPosition, targetPosition);
        float journeyLengthRot = Quaternion.Angle(initialRotation, Quaternion.Euler(targetRotationEulerAngles));

        while (Vector3.Distance(Camera.main.transform.position, targetPosition) > 0.001f ||
               Quaternion.Angle(Camera.main.transform.rotation, Quaternion.Euler(targetRotationEulerAngles)) > 0.001f)
        {
            float distCoveredPos = (Time.time - startTime) * moveSpeed;
            float fracJourneyPos = distCoveredPos / journeyLengthPos;
            float fracJourneyRot = fracJourneyPos; 

            Camera.main.transform.position = Vector3.Lerp(initialPosition, targetPosition, fracJourneyPos);
            Camera.main.transform.rotation = Quaternion.Lerp(initialRotation, Quaternion.Euler(targetRotationEulerAngles), fracJourneyRot);

            yield return null;
        }

        inSubmenu = newInSub;
        transform.rotation = defaultRotation;
    }
}
