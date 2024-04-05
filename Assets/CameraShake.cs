using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector3 originalPosition;
    bool canShake;
    float shakeFrequency = default;
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (canShake)
        {
            CameraShaker();
        }
    }

    public void CameraShaker()
    {
        transform.position = originalPosition + Random.insideUnitSphere * Time.deltaTime *shakeFrequency;
    }

    public void SetShake()
    {
        canShake = !canShake;
    }
}
