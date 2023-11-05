using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Buffers.Text;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

/// <summary> Script used to display damage numbers on the game </summary>
public class HitpointsRenderer : MonoBehaviour
{
    [Header("Hitpoints Container")]
    [SerializeField] private GameObject HitPointPrefab;

    [Header("Other Tools")]
    [SerializeField] private bool DisableHitpoints;
    [SerializeField] private bool TestAnimation;

    /// <summary> HitpointsRenderer's Singleton </summary>
    public static HitpointsRenderer Instance;

    //Local Variables
    private readonly float acceleration = 1600;
    private readonly float startingSpeed = 100;
    private readonly float hitpointStayTime = 1f;


    void Awake()
    {
        Instance = this; //Singleton set
        TestAnimation = false;

        // HACK: Render the first ever hitpoint behind the camera to circumvent
        // A bug where for some reason the first ever rendered hitpoint lags the game and also
        // Appears offset
        Vector3 moddedCameraPos = Camera.main.transform.position;
        moddedCameraPos += Vector3.back * 10f;
        PrintDamage(moddedCameraPos, 2, true);
    }

    //Testing
    void Update()
    {
        if (TestAnimation)
        {
            PrintDamage(Vector3.zero, 1, false);
            TestAnimation = false;
        }
    }

    // HACK: Because we are using "Screen Space - Camera" on the game, getting a point on the canvas
    // Is very difficult, so it creates a 3D object of the number in the world instead.
    // This solution, however, makes the number not face the camera the further along the left and right
    // Axis they are
    /// <summary> Display the damage number on the screen </summary>
    public void PrintDamage(Vector3 entityPos, int damage, bool isShield)
    {
        //Dont do anything if disabled
        if (DisableHitpoints) { return; }

        //Calculate the position of the hitpoint on the canvas based of the location of the entity
        float offsetZ = 6f; //towards the camera
        Vector3 newPos = new Vector3(entityPos.x, entityPos.y, entityPos.z - offsetZ);

        // Create the Hitpoint
        GameObject hitpoint = Instantiate(HitPointPrefab, newPos, Quaternion.identity, this.transform);

        // Get the TextMesh Component
        TMP_Text hitpointMesh = hitpoint.GetComponent<TMP_Text>();

        // Set the data
        hitpointMesh.text = damage.ToString();

        // Assign the color of the hitpoint, using magenta here because blue is
        // hard to see against the background
        if (isShield) { hitpointMesh.color = Color.blue; }
        else { hitpointMesh.color = Color.red; }

        StartCoroutine(AnimateHitpoint(hitpoint));
    }

    //Animation Coroutine
    private IEnumerator AnimateHitpoint(GameObject hitpoint)
    {
        float speed = startingSpeed;

        //Animates the hitpoint going up using gravity
        while (speed > -startingSpeed)
        {
            //Gravity
            speed -= Time.deltaTime * acceleration;

            //Animating up
            hitpoint.transform.position += speed * Time.deltaTime * Vector3.up;

            //Wait 1 frame
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Now hold number in position for a bit
        yield return new WaitForSeconds(hitpointStayTime);

        //Destroy the hitpoint object
        Destroy(hitpoint);
    }
}
