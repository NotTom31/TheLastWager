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

    //TO DELETE
    private IEnumerator Testing()
    {
        /*
        ContractGenerator cg = new ContractGenerator();
        cg.NewNodeByIndex(0, 0, 0, 0, 4); //spades are worth +1
        cg.NewNodeByIndex(0, 0, 1, 0, 7); //diamonds are worth +3
        cg.NewNodeByIndex(0, 0, 2, 0, 0); //clubs are worth -2
        cg.NewNodeByIndex(0, 0, 3, 0, 4); //hearts are worth +1 */

        yield return new WaitForSeconds(2);
        EvaluateCardPlay(Suit.Spade, true);
        yield return new WaitForSeconds(2);
        EvaluateCardPlay(Suit.Heart, false);
    }

    public void StartContract()
    {
        //TODO: pass reference to contract and invoke its clauses

        OnNewContract?.Invoke();
    }

    public void EvaluateCardPlay(Suit s, bool user) //if user is false, the card was played by the devil
    {
        pointReward = 0.0f;

        //events in this switch statement will modify pointReward from the ContractClause class
        switch (s)
        {
            case Suit.Spade:
                Debug.Log("play spade");
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
