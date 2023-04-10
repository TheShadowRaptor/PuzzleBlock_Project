using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class LevelManager : MonoBehaviour
{
    public List<Material> shadowBlocks = new List<Material>();
    public List<GameObject> shadowWaters = new List<GameObject>();
    public Material shadowWater;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Material material in shadowBlocks)
        {
            material.SetFloat("_Outline_DepthTightening", 0.2f);
            material.SetFloat("_Outline_Thickness", 0.01f);
        }

        foreach (GameObject shadoweWater in shadowWaters)
        {
            shadoweWater.GetComponent<MeshRenderer>().material = shadowWater;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
