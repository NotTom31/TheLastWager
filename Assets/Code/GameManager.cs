using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MAIN_MENU,
    PAUSE,
    BEGIN,
    DRAW,
    CONTRACT,
    BET,
    PLAY
}

public enum Turn
{
    PLAYER,
    DEVIL
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public delegate void StateChangeHandler(GameState state);
    public static event StateChangeHandler OnStateChange;

    public GameState gameState { get; private set; } = GameState.MAIN_MENU;
    public Turn turn { get; private set; } = Turn.PLAYER;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
    }

    public void SetState(GameState state)
    {
        gameState = state;
        OnStateChange?.Invoke(state); //prob a better way to do this
    }

    public void SetState(Turn state)
    {
        turn = state;
        //OnStateChange?.Invoke(state); //prob a better way to do this
    }

    public void StartGame()
    {
        SetState(GameState.BEGIN);
        SetState(Turn.PLAYER);
    }
}
