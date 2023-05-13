using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventObject : LevelBlock
{
    public static List<EventObject> eventObjects = new List<EventObject>();
    abstract public void PlayEvent();
    abstract public void CancelEvent();

    private void OnEnable()
    {
        eventObjects.Add(this);
    }

    private void OnDisable()
    {
        eventObjects.Remove(this);
    }
}
