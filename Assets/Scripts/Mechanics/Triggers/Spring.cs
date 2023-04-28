using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour, ICellOccupier
{
    public bool IsSolid { get => false; set { } }
    public int elevation = 2;
    public int distance = 2;
    public float springTime = 0.1f;
    public Vector3 GetPosition() => transform.position;

    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {
        Vector3Int originalDir = dir;
        dir = new Vector3Int(dir.x * distance, 0,dir.z * distance);
        Vector3Int endPos = Vector3Int.RoundToInt(entered.GetPosition() + dir + (Vector3.up * elevation));
        var endDesiredStart = GridCell.GetCell(endPos);
        bool gotBlocked = false;
        for (int i = 0; i < distance; i++) {
            if (endDesiredStart.hasAnySolid) {
                endPos = endPos - originalDir;
                endDesiredStart = GridCell.GetCell(endPos);
                gotBlocked = true;
            }
            else {
                break;
            }
        }
        Vector3Int originalEndPos = endPos;
        Debug.Log($"Player entered from dir {dir} current pos {entered.GetPosition()} end pos calculated {endPos}");
        if (!gotBlocked) {
            for (int i = 0; i < 50; i++)
            {
                var foundCell = GridCell.GetCell(endPos);
                if (!foundCell.hasAnySolid && foundCell.hasGround)
                {
                    break;
                }

                //THERE'S NO GROUND! KEEP FALLING!
                endPos += Vector3Int.down;
                if (i == 49) //DIDN'T FIND AN END!
                {
                    endPos = originalEndPos;
                    break;
                }
            }
        }

        entered.moveTween.Kill(false);
        entered.ForceSetNextCell(Vector3Int.RoundToInt(endPos));
        entered.moveTween = entered.transform.DOJump(endPos, elevation,1, springTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            entered.CompleteMovement();
        });
        //Vector3Int destination = dir;
        //entered.ForceMove(destination);
    }

    public void BlockExitHere(BlockCharacter exited)
    {
        
    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {

    }
}
