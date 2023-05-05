using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour, ICellOccupier
{
    protected GridCell _currentCell, _nextCell;
    public bool IsSolid { get => false; set { } }
    // Start is called before the first frame update
    void Start()
    {
        Vector3Int cellPos = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        transform.position = cellPos;
        _currentCell = GridCell.GetCell(cellPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void BlockEnteredHere(BlockCharacter entered, Vector3Int dir)
    {
        if (LightEmitter.IsInLightRange(transform.position) && entered == MasterSingleton.Instance.Player)
        {
            MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.win);
        }
    }

    public void BlockExitHere(BlockCharacter exited)
    {

    }

    public void OnBlockMoveAttemptFail(BlockCharacter attempt)
    {
        
    }
}
