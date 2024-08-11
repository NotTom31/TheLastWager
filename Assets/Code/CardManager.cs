using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public enum Suit
{
    Club,
    Diamond,
    Heart,
    Spade
}

public class CardManager : MonoBehaviour
{
    public static CardManager Instance;
    private Card selectedCard;
    public Card devilSelectedCard;

    public List<Card> playerTableCards = new List<Card>();
    public List<Card> playerHandCards = new List<Card>();
    public List<Card> devilTableCards = new List<Card>();
    public List<Card> devilHandCards = new List<Card>();

    public event Action<Card, bool> OnCardPlayed;
    public List<float> card_values = new List<float>();
    public List<Card> cardGroup = new List<Card>(50);

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
        //RandomizeAllSuitsAnimated();
        for (int i = 0; i <= 3; i++)
        {
            //print("hiiiiiii");
            card_values.Add(0f);
        }

    }

    private void Update()
    {
        foreach(Card card in cardGroup)
        {
            int i = 0;
            if (card.currentSuit == Suit.Club) {i = 0;}
            if (card.currentSuit == Suit.Diamond) {i = 1;}
            if (card.currentSuit == Suit.Diamond) {i = 2;}
            if (card.currentSuit == Suit.Diamond) {i = 3;}

            if (card_values[i] >= 0f)
            {
                card.Indicator.text = "+" + card_values[i].ToString();
            }
            else
            {
                card.Indicator.text = card_values[i].ToString();
            }
        }
    }

    [SerializeField] GameObject PlayerHand;
    [SerializeField] GameObject DevilHand;

    public void SelectCard(Card card)
    {
        if (GameManager.Instance.gameState != GameState.PLAY || card.isPlayersCard == false)
            return;

        if (selectedCard != null)
        {
            selectedCard.isCardSelected = false;
        }

        selectedCard = card;
        selectedCard.isCardSelected = true;

        card.ChangeCardPlayState();
    }

    public void PlayCard(Suit suit, bool isPlayersCard)
    {
        if (isPlayersCard)
        {
            SFXManager.Instance.QueueNextBeat("PlayerPlay", 1.0f, 0.1f);
            playerTableCards.Add(selectedCard);
            playerHandCards.Remove(selectedCard);
        }
        else
        {
            SFXManager.Instance.QueueNextBeat("DevilPlay", 1.0f, 0.1f);
            devilTableCards.Add(selectedCard);
            devilHandCards.Remove(selectedCard);
        }

        OnCardPlayed?.Invoke(selectedCard, isPlayersCard);
        //Debug.Log("here");
        ContractsManager.Instance.EvaluateCardPlay(suit, isPlayersCard);
        GameManager.Instance.SwitchTurn();
    }

    public void PlayCardDevil(Suit suit, bool isPlayersCard)
    {
        if (isPlayersCard)
        {
            playerTableCards.Add(devilSelectedCard);
            playerHandCards.Remove(devilSelectedCard);
        }
        else
        {
            devilTableCards.Add(devilSelectedCard);
            devilHandCards.Remove(devilSelectedCard);
        }

        OnCardPlayed?.Invoke(selectedCard, isPlayersCard);
        //Debug.Log("here");
        ContractsManager.Instance.EvaluateCardPlay(suit, isPlayersCard);
        GameManager.Instance.SwitchTurn();
    }

    public void UnPlayCard(Suit suit, bool isPlayersCard)
    {
        if (isPlayersCard)
        {
            playerTableCards.Remove(selectedCard);
            playerHandCards.Add(selectedCard);
        }
        else
        {
            devilTableCards.Remove(selectedCard);
            devilHandCards.Add(selectedCard);
        }
    }

    public void RandomizeAllSuitsAnimated() //use this for resetting all cards 
    {
        // Clear previous lists
        playerHandCards.Clear();
        playerTableCards.Clear();
        devilHandCards.Clear();
        devilTableCards.Clear();

        // Find and categorize cards based on their tag
        Card[] allCards = FindObjectsOfType<Card>();

        foreach (Card card in allCards)
        {
            card.canClick = false;
            card.ResetCard();

            Suit randomSuit = (Suit)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(Suit)).Length);
            card.SetSuit(randomSuit);
            Debug.Log(randomSuit + " " + card);


            if (card.isPlayersCard)
            {
                playerHandCards.Add(card);
                card.canClick = true;
            }
            else
            {
                devilHandCards.Add(card);
            }
        }

    }

    //public void RandomizeAllSuitsPart2() //This is called by the RandomizeAllCards function above (yes I know it's dumb lol)
    //{
    //    Card[] allCards = FindObjectsOfType<Card>();

    //    foreach (Card card in allCards)
    //    {
    //        Suit randomSuit = (Suit)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(Suit)).Length);
    //        card.SetSuit(randomSuit);

    //        if (card.isPlayersCard)
    //        {
    //            playerHandCards.Add(card);
    //            card.canClick = true;
    //        }
    //        else
    //        {
    //            devilHandCards.Add(card);
    //        }
    //    }
    //}
}
