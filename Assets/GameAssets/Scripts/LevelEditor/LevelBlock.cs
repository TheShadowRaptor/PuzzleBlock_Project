using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    public string blockID;
    public int hashID;
    public static List<LevelBlock> blocks = new List<LevelBlock>();

    // Start is called before the first frame update
    void Start()
    {
        blocks.Add(this);
    }

    private void OnDestroy()
    {
        blocks.Remove(this);   
    }
}
