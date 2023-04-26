using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : BlockCharacter, ICellOccupier
{
    protected void Start()
    {
        Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        transform.position = cellPos;
        _currentCell = GridCell.GetCell(cellPos);
    }
    //
    // protected void Update()
    // {
    //     //Get a new position based on the direction.
    //     Vector3Int potentialNewPosition = _currentCell.cellPos + Vector3Int.down;
    //     //Find the new cell
    //     GridCell newCell = GridCell.GetCell(potentialNewPosition);
    //     //Debug.Log($"Called movement {newCell.cellPos} is solid? {newCell.hasAnySolid}  non occupier? {newCell.hasNonOccupierSolid} occupier count {newCell.occupiers.Count}");
    //     bool hasSolidInNewCell = newCell.hasAnySolid;
    //
    //     Vector3Int newPotentialPosForBox = potentialNewPosition + Vector3Int.down;
    //     GridCell newBoxCell = GridCell.GetCell(newPotentialPosForBox);
    //
    //     if (newBoxCell.hasAnySolid)
    //     {
    //         for (int i = 0; i < newCell.occupiers.Count; i++)
    //         {
    //             //IT CAN MOVE TO NEW BOX POS!
    //             hasSolidInNewCell = false;
    //             var Box = newCell.occupiers[i];
    //             gameObject.transform.DOMove(newPotentialPosForBox, moveTime).OnComplete(() =>
    //             {
    //                 newCell.RefreshCellContents();
    //                 newBoxCell.RefreshCellContents();
    //             });
    //         }
    //     }
    // }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void BlockEnteredHere(PlayerControllerGrid entered, Vector3Int dir)
    {

    }

    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {

    }
}