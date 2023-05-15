using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        mainmenu,
        gameplay,
        paused,
        edit,
        win
    }

    [SerializeField] private GameState state;
    public GameState State { get => state; }

    private void Awake()
    {
        GridCell.all.Clear();
        SwitchGameState(GameState.mainmenu);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case GameState.mainmenu:
                break;

            case GameState.gameplay:
                break;

            case GameState.paused:
                break;

            case GameState.edit:
                break;

            case GameState.win:
                break;
        }
    }

    public void SwitchGameState(GameState _gameState)
    {
        switch (_gameState)
        {
            case GameState.mainmenu:
                state = GameState.mainmenu;
                break;

            case GameState.gameplay:
                state = GameState.gameplay;
                break;

            case GameState.paused:
                state = GameState.paused;
                break;

            case GameState.edit:
                state = GameState.edit;
                break;

            case GameState.win:
                state = GameState.win;
                break;
        }
    }
}
