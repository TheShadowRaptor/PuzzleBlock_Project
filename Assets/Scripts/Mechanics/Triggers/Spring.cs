using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : TriggerObject, ICellOccupier
{
    public bool IsSolid { get => false; set { } }
    public int elevation = 2;
    public int distance = 2;
    public float springTime = 0.1f;
    public Vector3 GetPosition() => transform.position;

    public void PlayerEnteredHere(PlayerControllerGrid entered, Vector3Int dir)
    {
        dir = new Vector3Int(dir.x * distance, elevation,dir.z * distance);
        entered.transform.DOJump(dir, 0.01f,1, springTime);
        //Vector3Int destination = dir;
        //entered.ForceMove(destination);
    }


    public void PlayerExitHere(PlayerControllerGrid exitted)
    {

    }
}
