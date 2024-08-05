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
    [SerializeField] GameObject Bet;
    [SerializeField] TimeTracking Timer;
    [SerializeField] SoulTracking SoulCount;
    [SerializeField] PointTracker points;
    [SerializeField] public TextMeshProUGUI contractClause1;
    [SerializeField] public TextMeshProUGUI contractClause2;
    [SerializeField] public TextMeshProUGUI contractClause3;
    [SerializeField] public TextMeshProUGUI contractClause4;
    [Space]
    [SerializeField] public TextMeshProUGUI MultiContractClause1;
    [SerializeField] public TextMeshProUGUI MultiContractClause2;
    [SerializeField] public TextMeshProUGUI MultiContractClause3;
    [SerializeField] public TextMeshProUGUI MultiContractClause4;
    [Space]
    [SerializeField] public TextMeshProUGUI MultiContractClause5;
    [SerializeField] public TextMeshProUGUI MultiContractClause6;
    [SerializeField] public TextMeshProUGUI MultiContractClause7;
    [SerializeField] public TextMeshProUGUI MultiContractClause8;
    [Space]
    [SerializeField] public TextMeshProUGUI MultiContractClause9;
    [SerializeField] public TextMeshProUGUI MultiContractClause10;
    [SerializeField] public TextMeshProUGUI MultiContractClause11;
    [SerializeField] public TextMeshProUGUI MultiContractClause12;

    public static MenuManager Instance;

    public int minBet = 5;
    public int maxBet = 15;
    private int betValue;
    public TMP_InputField inputField;
    public TextMeshProUGUI minBetText;
    public TextMeshProUGUI maxBetText;
    public Button betButton;

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
        inputField.onValueChanged.AddListener(delegate { ValidateInput(); });
        betButton.interactable = false;
    }

    public void ValidateInput()
    {
        string input = inputField.text;

        if (int.TryParse(input, out int number))
        {
            if (number >= minBet && number <= maxBet)
            {
                betButton.interactable = true;
                return;
            }
        }

        // Disable the button if input is not valid
        betButton.interactable = false;
    }

    public void OpenGameUI()
    {
        SwitchUI("GameUI");
        AudioManager.Instance.Fade("AmbianceBase", false);
        AudioManager.Instance.Fade("IntroBase", true);
        GameManager.Instance.SetState(GameState.BEGIN);
    }

    public void StartNewGame()
    {
        Debug.Log("here");
        GameManager.Instance.StartGame();
        OpenMultiContract();
    }

    public void OpenCredits()
    {
        //Click
        SwitchUI("Credits");
    }

    public void OpenMainMenu()
    {
        //Click
        SwitchUI("MainMenu");
    }

    public void OpenBet()
    {
        minBetText.text = "Minimum bet: " + minBet.ToString();
        maxBetText.text = "Maximum bet: " + maxBet.ToString();
        //Click
        SwitchUI("BetUI");
    }

    public void OpenOneContract()
    {
        //Click
        SwitchUI("OneContract");
    }


    public void CloseOneContract()
    {
        //Click
        SwitchUI("GameUI");
    }

    public void OpenMultiContract()
    {
        //Click
        SwitchUI("MultiContract");
        GameManager.Instance.SetState(GameState.CONTRACT);
    }

    public void SelectContract(int i)
    {
        ContractsManager.Instance.SelectContract(i);
        Debug.Log("selected number... " + i);
        SwitchUI("GameUI");
        GameManager.Instance.SetState(GameState.BET);
        OpenBet();
    }

    public void CheckWinner()
    {
        int result = points.ComparePoints();
        switch (result)
        {
            case 0:
                SoulCount.SetSoulCount(SoulCount.Souls + betValue * 2);
                break;
            case 1:
                SoulCount.SetSoulCount(SoulCount.Souls);
                break;
            case 2:
                SoulCount.SetSoulCount(SoulCount.Souls + betValue);
                break;
        }

        minBet += 5;
        maxBet += 15;

        if (SoulCount.Souls >= 100)
        {
            Debug.Log("WOW");
        }
        else if (SoulCount.Souls <= 0)
        {
            Debug.Log("Game Over");
        }
    }

    public void SubmitBet()
    {
        if (int.TryParse(inputField.text, out int number))
        {
            betValue = number;
        }

        SoulCount.SetSoulCount(SoulCount.Souls - betValue);

        BeginPlay();
    }

    public void BeginPlay()
    {
        SwitchUI("GameUI");
        Debug.Log("start timer");
        StartTimer();
        GameManager.Instance.SetState(GameState.PLAY);
        //GameManager.Instance.turn = Turn.PLAYER;
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
        Bet.SetActive(false);

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
            case "BetUI":
                Bet.SetActive(true);
                break;
        }
    }

    public void UpdateTimer(float t)
    {
        Timer.SetTimerLength(t);
    }

    public void StartTimer()
    {
        UpdateTimer(60);//temp
        Timer.StartTimer();
    }

    public void UpdateSoulCount(float c)
    {
        SoulCount.SetSoulCount(c);
    }

    public void ClickQuit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    public void UpdateContractText(int i, string s)
    {
        switch (i)
        {
            case 0:
                Debug.Log("o");
                contractClause1.text = s;
                break;
            case 1:
                Debug.Log("1");
                contractClause2.text = s;
                break;
            case 2:
                Debug.Log("2");
                contractClause3.text = s;
                break;
            case 3:
                Debug.Log("3");
                contractClause4.text = s;
                break;
            default:
                Debug.Log("catch");
                break;
        }
    }

    public void UpdateMultiContractText(int i, string s)
    {
        switch (i)
        {
            case 0:
                Debug.Log("0");
                MultiContractClause1.text = s;
                break;
            case 1:
                Debug.Log("1");
                MultiContractClause2.text = s;
                break;
            case 2:
                Debug.Log("2");
                MultiContractClause3.text = s;
                break;
            case 3:
                Debug.Log("3");
                MultiContractClause4.text = s;
                break;
            case 4:
                Debug.Log("4");
                MultiContractClause5.text = s;
                break;
            case 5:
                Debug.Log("5");
                MultiContractClause6.text = s;
                break;
            case 6:
                Debug.Log("6");
                MultiContractClause7.text = s;
                break;
            case 7:
                Debug.Log("7");
                MultiContractClause8.text = s;
                break;
            case 8:
                Debug.Log("8");
                MultiContractClause9.text = s;
                break;
            case 9:
                Debug.Log("9");
                MultiContractClause10.text = s;
                break;
            case 10:
                Debug.Log("10");
                MultiContractClause11.text = s;
                break;
            case 11:
                Debug.Log("11");
                MultiContractClause12.text = s;
                break;
            default:
                Debug.Log("catch");
                break;
        }
    }
}
