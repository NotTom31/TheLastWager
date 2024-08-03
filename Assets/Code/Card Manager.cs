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

    public void PlayCard(Suit suit, bool isPlayersCard)
    {
        
    }

    public void SelectCard(Card card)
    {
 
        Card[] allCards = FindObjectsOfType<Card>();
        foreach (var c in allCards)
        {
            c.isCardSelected = false;
        }

        card.isCardSelected = true;
        Debug.Log("Card selected: " + card.currentSuit);
    }
}
