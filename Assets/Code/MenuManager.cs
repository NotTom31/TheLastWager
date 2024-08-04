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
    [SerializeField] GameObject Contract;
    [SerializeField] GameObject MultiContract;
    [SerializeField] GameObject Instructions;

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

    public void OpenOneContract()
    {
        SwitchUI("OneContract");
    }

    public void OpenMultiContract()
    {
        SwitchUI("MultiContract");
    }

    public void OpenTutorial()
    {
        SwitchUI("Tutorial");
    }

    private void SwitchUI(string name)
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        GameUI.SetActive(false);
        Contract.SetActive(false);
        MultiContract.SetActive(false);
        Instructions.SetActive(false);

        switch (name)
        {
            case "GameUI":
                GameUI.SetActive(true);
                break;
            case "Credits":
                CreditsMenu.SetActive(true);
                break;
            case "MainMenu":
                MainMenu.SetActive(true);
                break;
            case "OneContract":
                Contract.SetActive(true);
                break;
            case "MultiContract":
                MultiContract.SetActive(true);
                break;
            case "Tutorial":
                Instructions.SetActive(true);
                break;
        }
    }

    public void ClickQuit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
