using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPiller : EventObject
{
    public List<Material> shadowBlocks = new List<Material>();
    public List<GameObject> shadowWaters = new List<GameObject>();
    public Material water;
    private float thickness = 0.001f;
    private float tightening = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tightening == 2) return;
        if (activated)
        {
            GraduallyDecreaseShadow();
            PlayEvent();
        }
    }

    protected override void PlayEvent()
    {
        foreach (Material material in shadowBlocks)
        {
            material.SetFloat("_Outline_DepthTightening", tightening);
            material.SetFloat("_Outline_Thickness", thickness);
        }
    }

    void GraduallyDecreaseShadow()
    {
        tightening += Time.deltaTime;
        if (tightening >= 2)
        {
            tightening = 2;
            thickness = 0;
            activated = false;
            foreach (GameObject shadoweWater in shadowWaters)
            {
                shadoweWater.GetComponent<MeshRenderer>().material = water;
            }
        }
    }
}
