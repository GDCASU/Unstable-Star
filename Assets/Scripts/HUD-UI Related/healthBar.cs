using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public GameObject healthBarBasePrefab;
    public float healthBarBaseWidth;
    public GameObject healthBarSegmentPrefab;
    public float healthBarSegmentWidth;
    public GameObject healthBarEndPrefab;
    public float scaleMultiplier;

    private int holdCurrentHealth;
    private int holdMaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        holdCurrentHealth = currentHealth;
        holdMaxHealth = maxHealth;
        setMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if (holdMaxHealth != maxHealth)
        {
            setMaxHealth(maxHealth);
            holdMaxHealth = maxHealth;
        }
        if (holdCurrentHealth != currentHealth)
        {
            setCurrentHealth(currentHealth);
            holdCurrentHealth = currentHealth;
        }
        
    }

    void setMaxHealth(int maxHealth)
    {
        if (maxHealth >= 1)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            GameObject healthBarBase = Instantiate(healthBarBasePrefab);
            healthBarBase.transform.SetParent(transform);
            healthBarBase.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
            if (currentHealth < 1)
                healthBarBase.GetComponent<healthBarElement>().setEmpty();

            float currentWidth = healthBarBaseWidth * scaleMultiplier;

            GameObject healthBarSegment;
            for (int i=1; i<maxHealth; i++)
            {
                healthBarSegment = Instantiate(healthBarSegmentPrefab);
                healthBarSegment.transform.SetParent(transform);
                healthBarSegment.GetComponent<RectTransform>().anchoredPosition = new Vector3(currentWidth, 0f, 0f);
                if (currentHealth < i+1)
                    healthBarSegment.GetComponent<healthBarElement>().setEmpty();

                currentWidth += healthBarSegmentWidth * scaleMultiplier;
            }

            GameObject healthBarEnd = Instantiate(healthBarEndPrefab);
            healthBarEnd.transform.SetParent(transform);
            healthBarEnd.GetComponent<RectTransform>().anchoredPosition = new Vector3(currentWidth, 0f, 0f);
        }
    }

    void setCurrentHealth(int currentHealth)
    {
        for (int i = 0; i < transform.childCount-1; i++)
        {
            healthBarElement hb = transform.GetChild(i).gameObject.GetComponent<healthBarElement>();
            if (i > currentHealth-1)
            {
                hb.setEmpty();
            }
            else
            {
                hb.setFull();
            }
        }
    }
}

