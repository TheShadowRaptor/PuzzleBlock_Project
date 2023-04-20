using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : TriggerObject
{
    [SerializeField] private int blocksToTravel = 5;
    // Update is called once per frame
    void Update()
    {
        if (IsActivated())
        {
            playerController.LandedOnSpring(Vector3.forward, blocksToTravel);
        }
    }
}
