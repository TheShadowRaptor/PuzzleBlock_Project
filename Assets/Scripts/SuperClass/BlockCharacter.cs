using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

public abstract class BlockCharacter : MonoBehaviour, ICellOccupier
{
    public Tween moveTween;
    protected Quaternion originalRotation;
    protected Quaternion desiredRotation;

    protected GridCell _currentCell, _nextCell;
    public float moveTime = 0.5f;
    public Renderer visual;

    protected bool isMoving = false;

    public bool IsSolid
    {
        get => true;
        set { }
    }

    protected virtual bool DoMove(Vector3Int dir, bool withRotation = true)
    {
        //If my tween is still playing return.
        if (moveTween != null && moveTween.IsPlaying()) return false;

        //Get a new position based on the direction.
        Vector3Int potentialNewPosition = _currentCell.cellPos + dir;
        //Find the new cell
        GridCell newCell = GridCell.GetCell(potentialNewPosition);
        //Debug.Log($"Called movement {newCell.cellPos} is solid? {newCell.hasAnySolid}  non occupier? {newCell.hasNonOccupierSolid} occupier count {newCell.occupiers.Count}");
        bool hasSolidInNewCell = newCell.hasAnySolid;

        if (!newCell.hasNonOccupierSolid)
        {
            for (int i = 0; i < newCell.occupiers.Count; i++)
            {
                if (newCell.occupiers[i] is Box)
                {
                    var Box = (Box)newCell.occupiers[i];
                    bool didBoxMove = Box.DoMove(dir,false);
                    if (didBoxMove) {
                        hasSolidInNewCell = false;
                        break;
                    }
                }
            }
        }

        //If new cell doesn't have solid move there.
        if (!hasSolidInNewCell) {
            //Set bool is moving to prevent new movements until this one completes.
            isMoving = true;
            //Let's set our next cell
            _nextCell = newCell;
            //For rotation, as kind of a hack, I'm always resetting my original rotation to be 0.
            originalRotation = Quaternion.identity;
            //I then set my desired rotation to be based on the direction.
            desiredRotation = FindNewRotation(dir);
            //I set my move tween to be the movement tween to my new cell. On Complete it will call CompleteMovement
            moveTween = (Tween)transform
                .DOMove(new Vector3(newCell.cellPos.x, newCell.cellPos.y, newCell.cellPos.z), moveTime)
                .SetEase(Ease.InOutQuad).OnComplete(CompleteMovement);
            //While the movement tween is updating, I do my rotation using the elapsed percentage of the tween to control what rotation I'm showing.
            if (withRotation) {
                moveTween.OnUpdate<Tween>((TweenCallback) (() =>
                {
                    transform.rotation =
                        Quaternion.Slerp(originalRotation, desiredRotation, moveTween.ElapsedPercentage());
                    //This is a mess, but I'm using
                    float absoluteHalfPercentage = Mathf.Abs(moveTween.ElapsedPercentage() - 0.5f);
                    visual.transform.position = transform.position +
                                                ((Mathf.InverseLerp(0.5f, 0f, absoluteHalfPercentage) * 0.25f) *
                                                 Vector3.up);
                }));
            }

            return true;
            //transform.DORotate(, moveTime).SetEase(Ease.InOutQuad);
        }

        return false;
    }

    public void ForceSetNextCell(Vector3Int newCellPos) {
        _nextCell = GridCell.GetCell(newCellPos);
        if (!_nextCell.occupiers.Contains(this)) _nextCell.occupiers.Add(this);
    }

    public void CompleteMovement()
    {
        Vector3Int prevPos = _currentCell.cellPos;
        Vector3Int nextPos = _nextCell.cellPos;
        Vector3Int dir = nextPos - prevPos;
        //Let old cell refresh its concents to realize it's not solid since my player isn't there anymore.
        _currentCell.RefreshCellContents();
        //If for some reason I think I'm still on previous cell, no i'm not.
        if (_currentCell.occupiers.Contains(this)) _currentCell.occupiers.Remove(this);
        for (int i = 0; i < _currentCell.occupiers.Count; i++) {
            _currentCell.occupiers[i].BlockExitHere(this);
        }

        //Refresh new cell contents to realize it's now solid since my player is there.
        _nextCell.RefreshCellContents();
        if (!_nextCell.occupiers.Contains(this)) _nextCell.occupiers.Add(this);
        //Set the current cell to be the new cell.
        _currentCell = _nextCell;
        for (int i = 0; i < _currentCell.occupiers.Count; i++) {
            _currentCell.occupiers[i].BlockEnteredHere(this, dir);
        }
        isMoving = false;
        var CellDown = GridCell.GetCell(_currentCell.cellPos + Vector3Int.down);
        if (!CellDown.hasAnySolid)
        {
            DoMove(Vector3Int.down);
        }
    }

    public Quaternion FindNewRotation(Vector3Int dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.z)) //going left or right
        {
            if (dir.x < 0) //going left
            {
                return Quaternion.AngleAxis(90, Vector3.forward);
            }
            else //going right
            {
                return Quaternion.AngleAxis(-90, Vector3.forward);
            }
        }
        else
        {
            //going forwards or backwards
            if (dir.z < 0) //going back
            {
                return Quaternion.AngleAxis(-90, Vector3.right);
            }
            else //going forward
            {
                return Quaternion.AngleAxis(90, Vector3.right);
            }
        }
    }

    public void ForceMove(Vector3Int direction)
    {
        isMoving = false;
        moveTween = null;
        DoMove(direction);
    }

    public virtual Vector3 GetPosition() { return transform.position; }
    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {
        
    }
    
    public void BlockExitHere(BlockCharacter exited)
    {
        
        
    }
}
