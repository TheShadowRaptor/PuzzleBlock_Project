using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerSpawnpoint : MonoBehaviour, ICellOccupier
{
    public bool IsSolid
    {
        get => false;
        set { }
    }

    protected GridCell _currentCell, _nextCell;
    public static PlayerSpawnpoint spawnpoint;
    // Start is called before the first frame update
    private void Awake()
    {
        spawnpoint = this;
    }

    protected void Start()
    {
        Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        transform.position = cellPos;
        _currentCell = GridCell.GetCell(cellPos);        
    }


    public Vector3 GetPosition() { return transform.position; }
    public Vector3Int GetPositionInt()
    {
        Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        return cellPos; 
    }
    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {
        // If checkpoints are ever used put them here
    }

    public void BlockExitHere(BlockCharacter exited)
    {


    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {

    }
    private void OnDrawGizmos()
    {
        Vector3 size = Vector3.one;
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
