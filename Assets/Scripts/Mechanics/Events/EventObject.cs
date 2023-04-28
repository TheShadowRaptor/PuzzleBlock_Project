using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EventObject : MonoBehaviour
{
    abstract public void PlayEvent();
    abstract public void CancelEvent();
}
