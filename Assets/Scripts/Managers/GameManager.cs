using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        mainmenu,
        gameplay,
        paused
    }

    private GameState state;
    public GameState State { get => state; }

    private void Awake()
    {
        GridCell.all.Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        SwitchGameState(GameState.mainmenu);
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
        }
    }
}
