using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ContractsManager : MonoBehaviour
{
    [SerializeField] PointTracker points;

    public event Action OnSpadesAddition;
    public event Action OnDiamondsAddition;
    public event Action OnClubsAddition;
    public event Action OnHeartsAddition;

    public event Action OnSpadesMultiplication;
    public event Action OnDiamondsMultiplication;
    public event Action OnClubsMultiplication;
    public event Action OnHeartsMultiplication;

    public float pointReward; //modified from other scripts through events
    
    public event Action OnNewContract;
    public event Action<bool> OnCheckHistory; //true for "it's the player's turn"

    List<Contract> activeContracts = new List<Contract>();
    List<Contract> prospectiveContracts = new List<Contract>();
    [SerializeField] GameObject contractPrefab;
    [SerializeField] Transform[] paperPositions;

    public static ContractsManager Instance;
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

    int scriptedClauseCount = 0;
    public void GenerateProspectiveContracts(int numClauses)
    {
        MenuManager.Instance.MultiContractClause1.text = string.Empty;
        MenuManager.Instance.MultiContractClause2.text = string.Empty;
        MenuManager.Instance.MultiContractClause3.text = string.Empty;
        MenuManager.Instance.MultiContractClause4.text = string.Empty;
        MenuManager.Instance.MultiContractClause5.text = string.Empty;
        MenuManager.Instance.MultiContractClause6.text = string.Empty;
        MenuManager.Instance.MultiContractClause7.text = string.Empty;
        MenuManager.Instance.MultiContractClause8.text = string.Empty;
        MenuManager.Instance.MultiContractClause9.text = string.Empty;
        MenuManager.Instance.MultiContractClause10.text = string.Empty;
        MenuManager.Instance.MultiContractClause11.text = string.Empty;
        MenuManager.Instance.MultiContractClause12.text = string.Empty;

        prospectiveContracts.Clear();
        for (int i = 0; i < 3; i++)
        {
            prospectiveContracts.Add(Instantiate(contractPrefab, transform).GetComponent<Contract>());
            for (int j = 0; j < numClauses; j++)
            {
                prospectiveContracts[i].SetCode(j, ContractGenerator.PRESET_CLAUSE_CODES[scriptedClauseCount]);
                scriptedClauseCount++;
            }
            prospectiveContracts[i].InitializeClauses();
            for (int j = 0; j < numClauses; j++)
                MenuManager.Instance.UpdateMultiContractText(i * 4 + j, prospectiveContracts[i].blurbs[j]);
        }
    }

    public void SelectContract(int i)
    {
        ActivateContract(prospectiveContracts[i]);
    }

    public void ActivateContract(Contract c)
    {
        prospectiveContracts.Clear();

        if (activeContracts.Count >= 5) //This is a bandaid fix to prevent the softlock, we will need to reevaluate this
        {
            activeContracts.RemoveAt(0);
        }

        activeContracts.Add(c);

        OnNewContract?.Invoke();

        c.ActivateClauses();
        c.transform.position = paperPositions[activeContracts.Count - 1].position;
        c.transform.rotation = paperPositions[activeContracts.Count - 1].rotation;
        c.transform.localScale = paperPositions[activeContracts.Count - 1].localScale;
    }

    public void NotifyContractExpiry(Contract caller)
    {
        activeContracts.Remove(caller);
        //TODO: if there's stuff that happens when you remove a contract, DO THAT HERE eg destroy object etc
    }

    public void EvaluateCardPlay(Suit s, bool user) //if user is false, the card was played by the devil
    {
        OnCheckHistory?.Invoke(user);
        pointReward = 0.0f;

        //events in this switch statement will modify pointReward from the ContractClause class
        switch (s)
        {
            case Suit.Spade:
                OnSpadesAddition?.Invoke();
                OnSpadesMultiplication?.Invoke();
                break;
            case Suit.Diamond:
                OnDiamondsAddition?.Invoke();
                OnDiamondsMultiplication?.Invoke();
                break;
            case Suit.Club:
                OnClubsAddition?.Invoke();
                OnClubsMultiplication?.Invoke();
                break;
            case Suit.Heart:
                OnHeartsAddition?.Invoke();
                OnHeartsMultiplication?.Invoke();
                break;
        }

        int finalCount = (int)Mathf.Round(pointReward);
        points.AddPoints(finalCount, user);
    }

    public void AddPoints(int amt, bool toPlayer)
    {
        points.AddPoints(amt, toPlayer);
    }
}
