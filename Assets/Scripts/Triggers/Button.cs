using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : TriggerObject
{
    public EventObject eventObject;

    // Update is called once per frame
    void Update()
    {
        if (IsActivated())  eventObject.activated = true;
    }
}
