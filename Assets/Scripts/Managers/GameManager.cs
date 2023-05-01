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

    void SwitchGameState(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.mainmenu:
                gameState = GameState.mainmenu;
                break;

            case GameState.gameplay:
                gameState = GameState.gameplay;
                break;

            case GameState.paused:
                gameState = GameState.paused;
                break;
        }
    }
}
