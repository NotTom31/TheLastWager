using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public List<Card> playerTableCards = new List<Card>();
    public List<Card> playerHandCards = new List<Card>();
    public List<Card> devilTableCards = new List<Card>();
    public List<Card> devilHandCards = new List<Card>();

    public event Action<Card, bool> OnCardPlayed;

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
        RandomizeAllSuitsAnimated();
    }

    [SerializeField] GameObject PlayerHand;
    [SerializeField] GameObject DevilHand;

    public void SelectCard(Card card)
    {
        if (selectedCard != null)
        {
            selectedCard.isCardSelected = false;
        }

        selectedCard = card;
        selectedCard.isCardSelected = true;
    }

    public void PlayCard(Suit suit, bool isPlayersCard)
    {
        if (isPlayersCard)
        {
            playerTableCards.Add(selectedCard);
            playerHandCards.Remove(selectedCard);
        }
        else
        {
            devilTableCards.Add(selectedCard);
            devilHandCards.Remove(selectedCard);
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
        }

    }

    public void RandomizeAllSuitsPart2() //This is called by the RandomizeAllCards function above (yes I know it's dumb lol)
    {
        Card[] allCards = FindObjectsOfType<Card>();

        foreach (Card card in allCards)
        {
            Suit randomSuit = (Suit)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(Suit)).Length);
            card.SetSuit(randomSuit);

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
}
