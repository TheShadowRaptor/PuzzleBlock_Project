using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggerObject : MonoBehaviour
{
    protected PlayerController playerController;

    protected void Start()
    {
        playerController = PlayerController.playerController;
    }

    protected bool IsActivated()
    {
        float distance = Vector3.Distance(playerController.gameObject.transform.position, this.gameObject.transform.position);
        if (distance < 1) return true;
        return false;
    }
}
