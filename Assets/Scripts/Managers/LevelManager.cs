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
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if (validSceneNames.Contains(scene.name))
        {
            int timeAllowedToSearch = 20;
            for (int i = 0; i < timeAllowedToSearch; i++)
            {
                SearchForSpawnpoints();
            }

            // Check if the loaded scene is the one where you want to spawn the player
            if (playerSpawnpoint == null)
            {
                Debug.Log("Could not find spawner");
                return;
            }

            SpawnPlayer();
        }
        OnDisable();
    }

    private void SpawnPlayer()
    {
        // Instantiate the player prefab at the spawn point
        MasterSingleton.Instance.Player._currentCell = GridCell.GetCell(playerSpawnpoint);
    }

    private void SearchForSpawnpoints()
    {
        playerSpawnpoint = GameObject.Find("PlayerSpawnpoint").GetComponent<PlayerSpawnpoint>().GetPositionInt();
    }
}
