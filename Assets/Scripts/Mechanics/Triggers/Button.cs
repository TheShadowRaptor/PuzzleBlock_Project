using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, ICellOccupier
{
    public bool IsSolid { get => false; set { } }
    public EventObject eventObject;
    public bool singlePush = false;

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {
        eventObject.PlayEvent();
    }

    public void BlockExitHere(BlockCharacter exited)
    {
        if (singlePush == false)
        {
            eventObject.CancelEvent();
        }
    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {

    }
}
