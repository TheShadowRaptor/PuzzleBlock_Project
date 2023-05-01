using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSingleton : MonoBehaviour
{
    static private MasterSingleton instance;
    static public MasterSingleton Instance { get => instance; }

    [Header("SingletonObjects")]
    // Managers
    private GameManager gameManager;
    private LevelManager levelManager;
    private InputManager inputManager;

    // GameCharacters
    private PlayerControllerGrid player;

    // Managers gets
    public GameManager GameManager { get => gameManager; }
    public LevelManager LevelManager { get => levelManager; }
    public InputManager InputManager { get => inputManager; }

    // GameCharacters gets
    public PlayerControllerGrid Player { get =>  player; }

    bool hasInstance;

    // Awake is called when object is first initialized
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            hasInstance = true;
        }

        else
        {
            Destroy(gameObject);

        }
        DontDestroyOnLoad(gameObject);

        // Find child gameobjects
        gameManager = GetComponentInChildren<GameManager>();
        levelManager = GetComponentInChildren<LevelManager>();
        inputManager = GetComponentInChildren<InputManager>();
        player = GetComponentInChildren<PlayerControllerGrid>();
    }

    private void OnDestroy()
    {
        hasInstance = false;
        instance = null;
    }
}
