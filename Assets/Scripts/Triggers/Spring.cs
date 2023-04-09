using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : TriggerObject
{
    [SerializeField] private float power = 1;
    [SerializeField] private float landPower = 1;
    [SerializeField] private float duration = 1;
    [SerializeField] private float landDuration = 1;
    // Update is called once per frame
    void Update()
    {
        if (IsActivated())
        {
            playerController.LandedOnSpring(power, landPower, duration, landDuration);
        }
    }
}
