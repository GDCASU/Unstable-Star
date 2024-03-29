using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndController : MonoBehaviour
{
    [Header("Menu Controller")]
    public bool isEnd;
    [SerializeField] private GameObject endObject;
    [SerializeField] Player player;

    [Header("Physics Variables")]
    private float acceleration = 0.01f;
    Vector3 minVelocity = new Vector3(0f, 0f, 0f);
    Vector3 maxVelocity;
    Vector3 currentVelocity;
    bool enterScreen = true;

    private void Start()
    {
        maxVelocity = new Vector3(0f, -acceleration * 20f, 0f);
    }

    private void Update()
    {
        if (isEnd)
        {
            endObject.SetActive(true);
            Time.timeScale = 0f;
            EnterScreen();
        }
        else
        {
            endObject.SetActive(false); 
            Time.timeScale = 1f;
        }

        if (player.GetHealth() <= 0)
        {
            isEnd = true;
        }

    }

    public void LoadMenu()
    {
        isEnd = false;
        SceneManager.LoadScene("Menu");
    }

    public void RestartScene()
    {
        isEnd = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void EnterScreen() // method that causes ship to enter screen
    {
        if (maxVelocity != minVelocity && enterScreen)
        {
            currentVelocity = Vector3.Lerp(maxVelocity, minVelocity, acceleration);
            endObject.transform.Translate(currentVelocity);
            maxVelocity = currentVelocity;
        }
        else
        {
            enterScreen = false;
            minVelocity = new Vector3(0f, 0f, 0f);
            maxVelocity = new Vector3(0f, -.3f, 0f);
        }
    }
}
