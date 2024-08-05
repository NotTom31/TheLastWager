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

        StartCoroutine(Testing());
    }

    int scriptedClauseCount = 0;
    public void GenerateProspectiveContracts(int numClauses)
    {
        MenuManager.Instance.contractClause1.text = string.Empty;
        MenuManager.Instance.contractClause2.text = string.Empty;
        MenuManager.Instance.contractClause3.text = string.Empty;
        MenuManager.Instance.contractClause4.text = string.Empty;

        prospectiveContracts.Clear();
        for (int i = 0; i < 3; i++)
        {
            prospectiveContracts.Add(Instantiate(contractPrefab).GetComponent<Contract>());
            for (int j = 0; j < numClauses; j++)
            {
                prospectiveContracts[i].SetCode(j, ContractGenerator.PRESET_CLAUSE_CODES[scriptedClauseCount]);
                scriptedClauseCount++;
                MenuManager.Instance.UpdateMultiContractText(i * 4 + j, prospectiveContracts[i].blurbs[j]);
            }
        }
    }

    //TO DELETE
    private IEnumerator Testing()
    {
        /*
        ContractGenerator cg = new ContractGenerator();
        cg.NewNodeByIndex(0, 0, 0, 0, 4); //spades are worth +1
        cg.NewNodeByIndex(0, 0, 1, 0, 7); //diamonds are worth +3
        cg.NewNodeByIndex(0, 0, 2, 0, 0); //clubs are worth -2
        cg.NewNodeByIndex(0, 0, 3, 0, 4); //hearts are worth +1 */

        /*
        yield return new WaitForSeconds(2);
        EvaluateCardPlay(Suit.Spade, true);
        yield return new WaitForSeconds(2);
        EvaluateCardPlay(Suit.Heart, false);
        */
        yield return null;
    }

    public void ActivateContract(Contract c)
    {
        prospectiveContracts.Clear();
        activeContracts.Add(c);

        OnNewContract?.Invoke();
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
