using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICellOccupier 
{
    public bool IsSolid
    {
        get => true;
        set { }
    }

    public Vector3 GetPosition();
    public void PlayerEnteredHere(PlayerControllerGrid entered, Vector3Int dir);
    public void PlayerExitHere(PlayerControllerGrid exited);
}
