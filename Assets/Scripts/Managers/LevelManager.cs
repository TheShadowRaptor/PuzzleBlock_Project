using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<string> validSceneNames = new List<string>();
    private Vector3Int playerSpawnpoint;

    private string previousScene;
    private string currentScene;

    Coroutine waitCoroutine = null;

    public string PreviousScene { get => previousScene; }
    public string CurrentScene { get => currentScene; }

    private void Update()
    {
        ToggleCoroutines();
        if (MasterSingleton.Instance.InputManager.Space == true)
        {
            SwitchScene("TestScene2");
            OnEnable();
        }
    }

    void ToggleCoroutines()
    {
        if (currentScene != "LoadingScene")
        {
            if (waitCoroutine != null) StopCoroutine(waitCoroutine);
            return;
        }

        GridCell.all.Clear();
        waitCoroutine = StartCoroutine(WaitInLoadingScene());     
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

    private void SpawnPlayer()
    {
        MasterSingleton.Instance.Player.Teleport(PlayerSpawnpoint.spawnpoint.GetPositionInt());
        MasterSingleton.Instance.Player.ResetStats();
        MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.gameplay);
    }

    IEnumerator WaitInLoadingScene()
    {
        if (currentScene == ("LoadingScene"))
        {
            // Do loading things here
            yield return 0.05f;
            Debug.Log($"LoadingScene - SwitchScene = {previousScene}");
            SwitchScene(previousScene);
            OnEnable();               
        }
    }
}
