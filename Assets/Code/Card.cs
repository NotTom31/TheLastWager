using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour
{
    public bool isPlayersCard;
    public bool isCardSelected = false;
    public bool canClick = true;
    public Suit currentSuit;
    private bool isInPlay;
    [SerializeField] GameObject Club;
    [SerializeField] GameObject Diamond;
    [SerializeField] GameObject Heart;
    [SerializeField] GameObject Spade;

    private bool isFlipped = false;
    private float flipDuration = 0.5f;
    private float slideDuration = 0.2f;
    private float slideDistance = 1f;
    private Vector3 cardOriginPosition;
    private Quaternion cardOriginRotation;
    public TextMeshPro Indicator;


    private bool added = false;
    private void Start()
    {
        Indicator = GetComponentInChildren<TextMeshPro>();
        SetSuit(Suit.Heart);
        cardOriginPosition = transform.position;
        cardOriginRotation = transform.rotation;
        if (!isPlayersCard)
        {
            FlipCardInstant();
            canClick = false;
        }
    }

    private void Update()
    {
        if (!added) {
            print(CardManager.Instance.cardGroup.Count);
            CardManager.Instance.cardGroup.Add(this);
            Indicator.text = CardManager.Instance.card_values[0].ToString();
            added = true;
        }
        if (isCardSelected && isPlayersCard && GameManager.Instance.gameState == GameState.PLAY)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                //FlipCard();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                //SetSuit(Suit.Club);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //SetSuit(Suit.Diamond);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                //SetSuit(Suit.Heart);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                //SetSuit(Suit.Spade);
            }
            if (Input.GetMouseButtonDown(0))
            {
                ChangeCardPlayState();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                //CardManager.Instance.RandomizeAllSuitsAnimated();
            }
            /*if (Input.GetKeyDown(KeyCode.W))
            {
                Debug.Log("Player Hand Cards:");
                foreach (Card card in CardManager.Instance.playerHandCards)
                {
                    Debug.Log(card.name + " - " + card.currentSuit);
                }

                Debug.Log("Devil Hand Cards:");
                foreach (Card card in CardManager.Instance.devilHandCards)
                {
                    Debug.Log(card.name + " - " + card.currentSuit);
                }
                Debug.Log("Player Table Cards:");
                foreach (Card card in CardManager.Instance.playerTableCards)
                {
                    Debug.Log(card.name + " - " + card.currentSuit);
                }

                Debug.Log("Devil Table Cards:");
                foreach (Card card in CardManager.Instance.devilTableCards)
                {
                    Debug.Log(card.name + " - " + card.currentSuit);
                }
            }*/
        }
        if (GameManager.Instance.gameState == GameState.PLAY)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    Card card = hit.collider.GetComponent<Card>();
                    if (card != null)
                    {
                        CardManager.Instance.SelectCard(card);
                    }
                }
            }
        }

    }

    public void ResetCard()
    {
        isInPlay = false;
        StartCoroutine(ResetCardCoroutine());
    }

    private IEnumerator ResetCardCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        while (elapsedTime < slideDuration)
        {
            transform.position = Vector3.Lerp(startPosition, cardOriginPosition, elapsedTime / slideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if (!isFlipped)
            FlipCard();
        yield return new WaitForSeconds(1.5f);
        //CardManager.Instance.RandomizeAllSuitsPart2();
        if (isPlayersCard)
        {
            FlipCard();
        }
    }

    public void SetSuit(Suit suit)
    {
        currentSuit = suit;
        UpdateActiveSuit();
    }

    private void ChangeCardPlayState()
    {
        //Shuffle sound
        if (isFlipped)
        {
            Debug.Log("Can't play upside down card! " + gameObject.name);
            return;
        }
        switch (isInPlay)
        {
            case false:
                isInPlay = true;
                StartCoroutine(PlayCardCoroutine(isInPlay));
                CardManager.Instance.PlayCard(currentSuit, isPlayersCard);
                break;
            case true:
                Debug.Log("Card already in play " + gameObject.name);
/*              isInPlay = false;
                StartCoroutine(PlayCardCoroutine(isInPlay));
                CardManager.Instance.UnPlayCard(currentSuit, isPlayersCard);*/
                break;
        }
    }

    private IEnumerator PlayCardCoroutine(bool isInPlay)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + transform.up * slideDistance;

        if (isInPlay)
        {
            while (elapsedTime < slideDuration)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / slideDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = endPosition;
        }
        else
        {
            while (elapsedTime < slideDuration)
            {
                transform.position = Vector3.Lerp(startPosition, cardOriginPosition, elapsedTime / slideDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            transform.position = cardOriginPosition;
        }
    }

    public void FlipCardInstant()
    {
        Quaternion startRotation = transform.rotation;
        if (!isFlipped)
            transform.rotation = startRotation * Quaternion.Euler(0f, 180f, 0f);
        else
            transform.rotation = cardOriginRotation;
        isFlipped = !isFlipped;
    }

    public void FlipCard()
    {
        StartCoroutine(FlipCardCoroutine());
    }

    private IEnumerator FlipCardCoroutine()
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;
        if (!isFlipped)
            endRotation = startRotation * Quaternion.Euler(0f, 180f, 0f);
        else
            endRotation = cardOriginRotation;

        while (elapsedTime < flipDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / flipDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        isFlipped = !isFlipped;
    }

    private void UpdateActiveSuit()
    {
        Club.SetActive(false);
        Diamond.SetActive(false);
        Heart.SetActive(false);
        Spade.SetActive(false);

        switch (currentSuit)
        {
            case Suit.Club:
                if (CardManager.Instance.card_values.Count > 0) Indicator.text = CardManager.Instance.card_values[0].ToString();
                Club.SetActive(true);
                break;
            case Suit.Diamond:
                if (CardManager.Instance.card_values.Count > 0) Indicator.text = CardManager.Instance.card_values[1].ToString();
                Diamond.SetActive(true);
                break;
            case Suit.Heart:
                if (CardManager.Instance.card_values.Count > 0) Indicator.text = CardManager.Instance.card_values[2].ToString();
                Heart.SetActive(true);
                break;
            case Suit.Spade:
                if (CardManager.Instance.card_values.Count > 0) Indicator.text = CardManager.Instance.card_values[3].ToString();
                Spade.SetActive(true);
                break;
        }
    }
}
