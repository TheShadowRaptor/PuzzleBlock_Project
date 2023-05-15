using Defective.JSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class LevelManager : MonoBehaviour
{
    public List<string> validSceneNames = new List<string>();
    public List<string> levels = new List<string>();
    public List<JSONObject> Levels = new List<JSONObject>();
    private Vector3Int playerSpawnpoint;

    private string previousScene;
    private string currentScene;

    public string PreviousScene { get => previousScene; }
    public string CurrentScene { get => currentScene; }

    private void Update()
    {
        
    }

    public void SwitchScene(string name)
    {
        if (validSceneNames.Contains(name) && currentScene != name)
        {
            SceneManager.LoadScene(name);
        }
    }

    private void OnEnable()
    {       
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (currentScene == scene.name) return;
        previousScene = currentScene;
        Debug.Log($"PreviousScene = {previousScene}");
        currentScene = scene.name;
        Debug.Log($"CurrentScene = {currentScene}");
        if (PlayerSpawnpoint.spawnpoint != null)
        {
            SpawnPlayer();
        }
    }

    public void SpawnPlayer()
    {
        MasterSingleton.Instance.Player.Teleport(PlayerSpawnpoint.spawnpoint.GetPositionInt());
        MasterSingleton.Instance.Player.ResetStats();
        //MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.gameplay);
    }
}
