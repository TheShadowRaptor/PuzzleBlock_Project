using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Serialization;
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

    public virtual void Serialize(JSONObject jsonObject)
    {    
        jsonObject.SetField("x", transform.position.x);
        jsonObject.SetField("y", transform.position.y);
        jsonObject.SetField("z", transform.position.z);
        jsonObject.SetField("hashID", hashID);
    }

    public virtual void DeSerialize(JSONObject jsonObject)
    {
        // Not yet
    }

    public virtual bool MiddleMouseEvent()
    {
        return false;
    }

}
