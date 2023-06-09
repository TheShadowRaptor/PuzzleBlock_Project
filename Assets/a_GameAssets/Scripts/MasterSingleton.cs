using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSingleton : MonoBehaviour
{
    static private MasterSingleton instance;
    static private bool hasInstance = false;
    static public MasterSingleton Instance
    {
        get
        {
            if (!hasInstance)
            {
                GameObject prefab = Resources.Load<GameObject>("MasterSingleton");
                instance = Instantiate(prefab).GetComponent<MasterSingleton>();
                hasInstance = true;
            }
            return instance;
        }
    }

    [Header("SingletonObjects")]
    // Managers
    private GameManager gameManager;
    private LevelManager levelManager;
    private InputManager inputManager;
    private AudioManager audioManager;
    private UIManager uIManager;

    // GameCharacters
    private PlayerControllerGrid player;

    // Managers gets
    public GameManager GameManager { get => gameManager; }
    public LevelManager LevelManager { get => levelManager; }
    public InputManager InputManager { get => inputManager; }
    public AudioManager AudioManager { get => audioManager; }
    public UIManager UIManager { get => uIManager; }

    // GameCharacters gets
    public PlayerControllerGrid Player { get =>  player; }

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
        audioManager = GetComponentInChildren<AudioManager>();
        uIManager = GetComponentInChildren<UIManager>();
        player = GetComponentInChildren<PlayerControllerGrid>();
    }

    private void OnDestroy()
    {
        hasInstance = false;
        instance = null;
    }
}
