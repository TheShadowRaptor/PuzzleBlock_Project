using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBlock : MonoBehaviour, ICellOccupier, LighListener
{
    private void OnEnable() { Lightpillar.lightListeners.Add(this); }
    private void OnDisable() { Lightpillar.lightListeners.Remove(this); }

    public void LightHasChanged() {
        bool checkSolid = IsSolid;
    }

    public void SolidityHasChanged() {
        var currentCell = GridCell.GetCell(Vector3Int.RoundToInt(GetPosition()));
        //Check if anyone should fall.
        if (!currentCell.hasAnySolid)
        {
            Vector3Int above = currentCell.cellPos + Vector3Int.up;
            GridCell aboveCell = GridCell.GetCell(above);
            for (int i = 0; i < aboveCell.occupiers.Count; i++) {
                if (aboveCell.occupiers[i] is BlockCharacter) {
                    ((BlockCharacter) aboveCell.occupiers[i]).DoMove(Vector3Int.down, false);
                }
            }
        }

    }

    private bool lastTimeCheckedSolid = false;
    public bool IsSolid
    {
        get {
            var tempLastTimeCheckedSolid = !LightEmitter.IsInLightRange(transform.position);
            if (tempLastTimeCheckedSolid == lastTimeCheckedSolid) return lastTimeCheckedSolid;
            lastTimeCheckedSolid = tempLastTimeCheckedSolid;
            SolidityHasChanged();
            return lastTimeCheckedSolid;
        }
        set { }
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
