using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventObject : MonoBehaviour
{
    public bool activated = false;
    abstract protected void PlayEvent();
}
