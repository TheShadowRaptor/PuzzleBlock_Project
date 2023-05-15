using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSaveSystem : MonoBehaviour
{
    public static Dictionary<int, LevelBlock> prefabDic = new Dictionary<int, LevelBlock>();
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
    public void DoSerialize()
    {
        level = Serialize();
    }

    public static string Serialize()
    {
        // Parent container
        var save = new JSONObject();

        for (int i = 0;i < LevelBlock.blocks.Count; i++)
        {
            // Child container
            var blockInfo = new JSONObject();
            LevelBlock block = LevelBlock.blocks[i];
            // Save blockinfo to child
            block.Serialize(blockInfo);
            save.Add(blockInfo);           
        }

        // return child infor back to parent
        return save.Print();
    }

    [ContextMenu("DeSerialize")]
    public void DoDeSerialize()
    {
        DeSerialize(level);
    }

    public static void DeSerialize(string str)
    {
        // Parent container
        var save = JSONObject.Create(str);
        for (int i = LevelBlock.blocks.Count-1; i >= 0; i--)
        {
            // clean up
            GameObject.Destroy(LevelBlock.blocks[i].gameObject);
        }

        for (int i = 0; i < save.list.Count; i++) 
        {
            // Child container
            var blockInfo = save.list[i];

            // Find block info in each Child container
            blockInfo.GetField(out float x, "x" ,0);
            blockInfo.GetField(out float y, "y", 0);
            blockInfo.GetField(out float z, "z", 0);
            blockInfo.GetField(out int hashID, "hashID", 0);

            if (prefabDic.TryGetValue(hashID, out LevelBlock levelblock))
            {
                // Spawn block and give them information within child container
                LevelBlock levelblockSpawned = Instantiate(levelblock, new Vector3(x, y, z), Quaternion.identity);
                levelblockSpawned.gameObject.transform.parent = LevelArea.Instance.levelAreaObj.transform;
                levelblockSpawned.DeSerialize(blockInfo);
            }
        }
    }
}
