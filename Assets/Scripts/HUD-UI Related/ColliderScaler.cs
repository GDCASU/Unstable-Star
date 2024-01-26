using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[ExecuteInEditMode]
/// <summary> Scales the size of the colliders on the border </summary>
public class ColliderScaler : MonoBehaviour
{
    // Private fields
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject upWall;
    [SerializeField] private GameObject downWall;
    private BoxCollider colliderRightWall;
    private BoxCollider colliderLeftWall;
    private BoxCollider colliderUpWall;
    private BoxCollider colliderDownWall;
    Vector3 verticalVector = new Vector3(1,0,100);
    Vector3 horizontalVector = new Vector3(0,1,100);
    float xFloat;
    float yFloat;

    private void Awake()
    {
        // Obtain Colliders from gameObjects
        colliderRightWall = rightWall.GetComponent<BoxCollider>();
        colliderDownWall = downWall.GetComponent<BoxCollider>();
        colliderLeftWall = leftWall.GetComponent<BoxCollider>();
        colliderUpWall = upWall.GetComponent<BoxCollider>();
    }


    // Meant to run once every time the editor loads the scene
    // FIXME: SHOULD BE CHANGED TO START WHEN EVERYTHING IS FINISHED (no more debugging or new aspect ratios)
    void Update()
    {
        // Obtain the Rect Tranform of the canvas
        xFloat = canvasObject.GetComponent<RectTransform>().rect.width + 5;
        yFloat = canvasObject.GetComponent<RectTransform>().rect.height + 200;

        // Speed up vector process
        verticalVector.y = yFloat;
        horizontalVector.x = xFloat;

        // Set the sizes
        colliderRightWall.size = verticalVector;
        colliderLeftWall.size = verticalVector;
        colliderUpWall.size = horizontalVector;
        colliderDownWall.size = horizontalVector;
    }


}
