using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<string> validSceneNames = new List<string>();
    private Vector3Int playerSpawnpoint;


    private void Update()
    {
        if (MasterSingleton.Instance.InputManager.Space == true)
        {
            SwitchScene("TestScene2");
            OnEnable();
        }
    }

    public void SwitchScene(string name)
    {
        if (validSceneNames.Contains(name))
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
        if (PlayerSpawnpoint.spawnpoint != null)
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        // Instantiate the player prefab at the spawn point
        MasterSingleton.Instance.Player._currentCell = GridCell.GetCell(PlayerSpawnpoint.spawnpoint.GetPositionInt());
        MasterSingleton.Instance.GameManager.SwitchGameState(GameManager.GameState.gameplay);
    }
}
