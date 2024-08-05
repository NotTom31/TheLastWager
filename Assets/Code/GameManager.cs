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






public class GameManager : MonoBehaviour
{
    int DrawCount = 0;
    int ContractCount = 0;
    int BetCount = 0;
    int PlayCount = 0;

    public static GameManager Instance;
    public delegate void StateChangeHandler(GameState state);
    public static event StateChangeHandler OnStateChange;

    public GameState gameState { get; private set; } = GameState.MAIN_MENU;

    GameState PrevState = GameState.MAIN_MENU;
    private void Update()
    {
        if (PrevState != gameState)
        {
            if (gameState == GameState.DRAW)
            {
                DrawCount++;
                if(DrawCount == 1)
                {
                    
                }

                if (DrawCount == 2)
                {
                    AudioManager.Instance.Fade("IntroSynth", true);
                }

            }

            if (gameState == GameState.CONTRACT)
            {
                SFXManager.Instance.QueueNextBeat("Paper", 1.0f, 0.3f, true);
                ContractCount++;
                if(ContractCount == 1)
                {
                    AudioManager.Instance.Queue("IntroHeartbeat", true);
                }
                
                if(ContractCount == 3)
                {
                    AudioManager.Instance.Queue("TimerMelody", true);
                }

                if (ContractCount == 4)
                {
                    AudioManager.Instance.Queue("TimerBass", true);
                }
                if (ContractCount == 6)
                {
                    AudioManager.Instance.Queue("TimerGroove", true);
                }

                if (ContractCount == 8)
                {
                    AudioManager.Instance.Queue("TimerChant", true);
                }

            }
            
            if (gameState == GameState.BET)
            {
                BetCount++;
                if (BetCount == 1)
                {

                }
            }

            if (gameState == GameState.PLAY)
            {
                PlayCount++;
                if (PlayCount == 1)
                {
                    AudioManager.Instance.Queue("IntroOctUp", true);
                }
                if (PlayCount == 2)
                {
                    AudioManager.Instance.Queue("TimerBuild", true);
                    StartCoroutine(aitw());
                }
            }



        }
        PrevState = gameState;
    }

    IEnumerator aitw()
    {
        yield return new WaitForSeconds(16.0f);
        AudioManager.Instance.Queue("TimerStart", true);
        AudioManager.Instance.Queue("TimerBass", true);
        AudioManager.Instance.Queue("IntroBase", false);
        AudioManager.Instance.Queue("IntroHeartbeat", false);
        AudioManager.Instance.Queue("IntroOctUp", false);
        AudioManager.Instance.Queue("IntroSynth", false);
    }

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
}
