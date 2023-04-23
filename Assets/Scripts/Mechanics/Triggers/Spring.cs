using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : TriggerObject
{
    [SerializeField] private int blocksToTravel = 5;
    [SerializeField] bool isElevationSpring = false;
    // Update is called once per frame
    void Update()
    {
        if (IsActivated())
        {
            playerController.LandedOnSpring(gameObject.transform.forward, blocksToTravel, isElevationSpring);
        }
    }
}
