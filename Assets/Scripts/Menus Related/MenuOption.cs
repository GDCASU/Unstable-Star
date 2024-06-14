using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuOption : MonoBehaviour // changes behaviours of options part of menu
{
    [Header("References")]
    [SerializeField] MenuManager menuManager;

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
            ttText.newText = "Welcome";
            ResetCamera();
        }

    }

    /// <summary>
    /// Activates this current option.
    /// </summary>
    public void SelectOption()
    {
        if (optionName == "Credits") ScenesManager.instance.LoadScene(Scenes.Credits);

        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveCamera(cameraPos, cameraRotation, true));
        inSubmenu = true;
        menuManager.SetCurrentMenu(CurrentMenu.PrimaryOptions);
    }

    /// <summary>
    /// Puts the camera back to the original position.
    /// </summary>
    public void ResetCamera()
    {
        if (moveCoroutine != null) StopCoroutine(moveCoroutine);
        moveCoroutine = StartCoroutine(MoveCamera(defaultCameraPos, defaultCameraRotation, false));
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0)) SelectOption();
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
            ttText.newText = "Welcome";
        }
            
    }

    IEnumerator MoveCamera(Vector3 targetPosition, Vector3 targetRotationEulerAngles, bool newInSub)
    {
        float startTime = Time.time;

        Vector3 initialPosition = Camera.main.transform.position;
        Quaternion initialRotation = Camera.main.transform.rotation;

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
