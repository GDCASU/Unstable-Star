using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceUiControls : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerInput.instance.ActivateUiControls();
    }

    private void OnDestroy()
    {
        PlayerInput.instance.ActivateShipControls();
    }
}
