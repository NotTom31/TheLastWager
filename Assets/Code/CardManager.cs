using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Implement play card logic if needed
    }

    public void RandomizeAllCards()
    {
        Card[] allCards = FindObjectsOfType<Card>();

        foreach (Card card in allCards)
        {
            Suit randomSuit = (Suit)Random.Range(0, System.Enum.GetValues(typeof(Suit)).Length);
            card.SetSuit(randomSuit);
        }
    }
}
