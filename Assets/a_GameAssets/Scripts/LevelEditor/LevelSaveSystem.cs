using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LevelSaveSystem : MonoBehaviour
{
    public static Dictionary<int, LevelBlock> prefabDic = new Dictionary<int, LevelBlock>();
    [SerializeField] private List<LevelBlock> levelBlocks = new List<LevelBlock>();
    [SerializeField] private string level;
    static public string lastLoadedFilename;
    static public string LastSavedLevel;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < levelBlocks.Count; i++)
        {
            levelBlocks[i].hashID = Animator.StringToHash(levelBlocks[i].blockID);
            prefabDic[levelBlocks[i].hashID] = levelBlocks[i];
        }
        lastLoadedFilename = "Level0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static string Serialize()
    {
        // Parent container
        var save = new JSONObject();

        for (int i = 0; i < LevelBlock.blocks.Count; i++)
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

    public static string lastLoadedString;
    public static void DeSerialize(string str)
    {
        lastLoadedString = str;
        // Parent container
        var save = JSONObject.Create(str);
        for (int i = LevelBlock.blocks.Count - 1; i >= 0; i--)
        {
            // clean up
            GameObject.Destroy(LevelBlock.blocks[i].gameObject);
        }

        for (int i = 0; i < save.list.Count; i++)
        {
            // Child container
            var blockInfo = save.list[i];

            // Find block info in each Child container
            blockInfo.GetField(out float x, "x", 0);
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

    [ContextMenu("FindNextLevel")]
    static public void FindNextLevel()
    {
        int i;
        for (i = 0; i < lastLoadedFilename.Length; i++)
        {
            if (!Char.IsLetter(lastLoadedFilename[i])) // check if the current character is not a letter
            {
                break; // we found the index where it stops being letters
            }
        }
        string levelName = lastLoadedFilename.Substring(0, i); // extract the non-number part from the beginning to i-1
        int levelNumber = Int32.Parse(lastLoadedFilename.Substring(i)); // extract the number part from i to the end and parse as an int
        Console.WriteLine("Level name: " + levelName); // Output: "Level name: level"
        Console.WriteLine("Level number: " + levelNumber); // Output: "Level number: 1"

        string nextLevel = $"{levelName}{levelNumber + 1}";
        string nextLevelFilePath = $"{Application.streamingAssetsPath}/{nextLevel}.txt";
        bool doesFileExist = File.Exists(nextLevelFilePath);
        Debug.Log($"Next level is {nextLevel} filepath is {nextLevelFilePath} does file exist? {doesFileExist}");
       
        var readText = File.ReadAllText(nextLevelFilePath);
        DeSerialize(readText);
        MasterSingleton.Instance.LevelManager.SpawnPlayer();
        lastLoadedFilename = nextLevel;
        Debug.Log($"Level = {nextLevel}");

        LastSavedLevel = nextLevel;
    }

    static public void LoadSavedLevel()
    {
        string levelName = LastSavedLevel;

        string nextLevel = $"{levelName}";
        string nextLevelFilePath = $"{Application.streamingAssetsPath}/{nextLevel}.txt";
        bool doesFileExist = File.Exists(nextLevelFilePath);

        var readText = File.ReadAllText(nextLevelFilePath);
        DeSerialize(readText);
        MasterSingleton.Instance.LevelManager.SpawnPlayer();
    }
}