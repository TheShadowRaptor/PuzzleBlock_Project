using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBlock : MonoBehaviour, ICellOccupier
{
    public bool IsSolid
    {
        get
        {
            return !LightEmitter.IsInLightRange(transform.position);
        }
        set { }
    }

    private void Update()
    {
        
    }

    public virtual Vector3 GetPosition() { return transform.position; }

    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {

    }

    public void BlockExitHere(BlockCharacter exited)
    {

    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {

    }
}
