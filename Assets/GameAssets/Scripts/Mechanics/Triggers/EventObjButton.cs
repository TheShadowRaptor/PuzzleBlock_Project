using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObjButton : LevelBlock, ICellOccupier
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
        FindEventObject();
        if (eventObject == null) return;
        eventObject.PlayEvent();
    }

    public void BlockExitHere(BlockCharacter exited)
    {
        if (eventObject == null) return;
        if (singlePush == false)
        {
            eventObject.CancelEvent();
        }
    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {

    }

    public void FindEventObject()
    {
        if (eventObject != null) return;
        for (int i = 0; i < EventObject.eventObjects.Count; i++)
        {
            if (Vector3Int.RoundToInt(EventObject.eventObjects[i].transform.position) == eventInformation)
            {
                eventObject = EventObject.eventObjects[i];
                return;
            }
        }
    }

    public Vector3Int eventInformation = new Vector3Int();
    public override void Serialize(JSONObject jsonObject)
    {
        base.Serialize(jsonObject);

        // Save Event info
        if (eventObject != null)
        {
            // Save to eventInformation
            jsonObject.SetField("xEvent", eventInformation.x);
            jsonObject.SetField("yEvent", eventInformation.y);
            jsonObject.SetField("zEvent", eventInformation.z);
        }
    }

    public override void DeSerialize(JSONObject jsonObject)
    {
        base.DeSerialize(jsonObject);

        // Load Event info
        jsonObject.GetField(out float x, "xEvent", 0);
        jsonObject.GetField(out float y, "yEvent", 0);
        jsonObject.GetField(out float z, "zEvent", 0);

        // Load from eventInformation
        eventInformation = new Vector3Int((int)x, (int)y, (int)z);
    }

    public override bool OpenMenu()
    {
        MasterSingleton.Instance.UIManager.ShowLevelEditorConnectButtonMenu(this);
        return true;
    }
}
