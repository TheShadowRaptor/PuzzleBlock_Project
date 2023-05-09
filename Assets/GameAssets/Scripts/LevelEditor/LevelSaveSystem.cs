using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSaveSystem : MonoBehaviour
{
    Dictionary<int, LevelBlock> prefabDic = new Dictionary<int, LevelBlock>();
    [SerializeField] private List<LevelBlock> levelBlocks = new List<LevelBlock>();
    [SerializeField] private string level;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < levelBlocks.Count; i++)
        {
            levelBlocks[i].hashID = Animator.StringToHash(levelBlocks[i].blockID);
            prefabDic[levelBlocks[i].hashID] = levelBlocks[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Serialize")]
    void Serialize()
    {
        var save = new JSONObject();

        for (int i = 0;i < LevelBlock.blocks.Count; i++)
        {
            var blockInfo = new JSONObject();
            blockInfo.SetField("x", LevelBlock.blocks[i].transform.position.x);
            blockInfo.SetField("y", LevelBlock.blocks[i].transform.position.y);
            blockInfo.SetField("z", LevelBlock.blocks[i].transform.position.z);
            blockInfo.SetField("hashID", LevelBlock.blocks[i].hashID);

            save.Add(blockInfo);
            
        }

        level = save.Print();
    }

    [ContextMenu("DeSerialize")]
    void DeSerialize()
    {
        var save = JSONObject.Create(level);
        for (int i = LevelBlock.blocks.Count-1; i >= 0; i--)
        {
            GameObject.Destroy(LevelBlock.blocks[i].gameObject);
        }

        for (int i = 0; i < save.list.Count; i++) 
        {
            var blockInfo = save.list[i];

            blockInfo.GetField(out float x, "x" ,0);
            blockInfo.GetField(out float y, "y", 0);
            blockInfo.GetField(out float z, "z", 0);
            blockInfo.GetField(out int hashID, "hashID", 0);

            if (prefabDic.TryGetValue(hashID, out LevelBlock levelblock))
            {
                Instantiate(levelblock.gameObject, new Vector3(x, y, z), Quaternion.identity);
            }
        }
    }
}
