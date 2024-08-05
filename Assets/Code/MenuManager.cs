using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject CreditsMenu;
    [SerializeField] GameObject GameUI;

    public static MenuManager Instance;

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

    private void Start()
    {
        OpenMainMenu();
    }

    public void OpenGameUI()
    {
        SwitchUI("GameUI");
        AudioManager.Instance.Fade("AmbianceBase", false);
        AudioManager.Instance.Fade("IntroBase", true);
        GameManager.Instance.SetState(GameState.BEGIN);
    }

    public void OpenCredits()
    {
        SwitchUI("Credits");
    }

    public void OpenMainMenu()
    {
        SwitchUI("MainMenu");
    }

    private void SwitchUI(string name)
    {
        switch (name)
        {
            case "GameUI":
                MainMenu.SetActive(false);
                CreditsMenu.SetActive(false);
                GameUI.SetActive(true);
                break;
            case "Credits":
                MainMenu.SetActive(false);
                CreditsMenu.SetActive(true);
                GameUI.SetActive(false);
                break;
            case "MainMenu":
                MainMenu.SetActive(true);
                CreditsMenu.SetActive(false);
                GameUI.SetActive(false);
                break;
        }
    }

    public void ClickQuit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
