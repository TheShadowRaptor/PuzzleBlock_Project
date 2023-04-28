using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : EventObject, ICellOccupier
{
    Animator animator;
    protected GridCell _currentCell, _nextCell;
    bool activatedOpen = false;
    bool activatedClose = false;

    // Start is called before the first frame update
    void Start()
    {
        Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        transform.position = cellPos;
        _currentCell = GridCell.GetCell(cellPos);

        animator = transform.GetChild(0).GetComponent<Animator>();
        animator.SetBool("false", true);
    }

    public void Update()
    {
        
    }

    public override void PlayEvent()
    {
        // Open Gate
        animator.SetBool("Open", true);
        activatedOpen = true;
    }

    public override void CancelEvent()
    {
        animator.SetBool("Open", false);
        activatedClose = true;
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
