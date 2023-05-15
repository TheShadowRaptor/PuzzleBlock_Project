using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
    public static Dictionary<Vector3Int, GridCell> all = new Dictionary<Vector3Int, GridCell>();
    public Vector3Int cellPos;
    public List<ICellOccupier> occupiers = new List<ICellOccupier>();

    //This getter and setter will return if there's a non occupier solid. Or will iterate trough the current occcupiers checking if they're solid. If none of those are solid it will return false;.
    public bool hasAnySolid
    {
        get
        {
            //If I have a non occupier solid return true;
            if (hasNonOccupierSolid) return true;
            //Iterate trough current occupiers and check if any of them are solid, return true if they are
            for (int i = 0; i < occupiers.Count; i++)
                if (occupiers[i].IsSolid) return true;
            //None of those happened so it's false, this doesn't have any solid.
            return false;
        }
    }

    public bool hasNonOccupierSolid
    {
        get
        {
            Collider[] hitColliders = Physics.OverlapBox(new Vector3(cellPos.x, cellPos.y, cellPos.z), Vector3.one * 0.45f, Quaternion.identity, ~0);
            //Iterate trough them.
            for (int i = 0; i < hitColliders.Length; i++)
                if (!hitColliders[i].TryGetComponent<ICellOccupier>(out ICellOccupier cellOccupier))
                    return true;

            return false;
        }
    }

    //This bool will check the cell underneath if it has a solid.
    public bool hasGround => GetCell(new Vector3Int(cellPos.x, cellPos.y - 1, cellPos.z)).hasAnySolid;

    public static GridCell GetCell(Vector3Int queryPos)
    {
        //If our dictionary already contains this cell it means we already created it and found the relevant information, just return it.
        if (all.ContainsKey(queryPos)) return all[queryPos];
        //If the code got here it means this cell isn't in the dictionary yet. We need to create and populate it with relevant information.
        GridCell newCell = new GridCell();
        newCell.cellPos = queryPos;
        newCell.RefreshCellContents();
        //Set it to the dictionary on that position.
        all[queryPos] = newCell;
        return newCell;
    }

    public void RefreshCellContents()
    {
        //Make sure occupiers are cleared when refreshing cell contents.
        occupiers.Clear();
        //Find things occupying that space. Make it a little smaller than a box to not hit adjascent grid spaces.
        Collider[] hitColliders = Physics.OverlapBox(new Vector3(cellPos.x, cellPos.y, cellPos.z), Vector3.one * 0.45f,
            Quaternion.identity, ~0);
        //Iterate trough them.
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //See if they have a cell occupier script, if they do find it and put it in the list, and if it's solid mark this cell as having a solid.
            if (hitColliders[i].TryGetComponent<ICellOccupier>(out ICellOccupier cellOccupier))
            {
                //Check if it's in range
                if ((cellPos - cellOccupier.GetPosition()).sqrMagnitude < 0.86f)
                    occupiers.Add(cellOccupier);
            }
        }
        //Debug.Log($"Refreshing cell concents for {cellPos} hassolid? {hasAnySolid}");
    }
}



