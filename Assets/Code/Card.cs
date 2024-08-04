using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public bool isPlayersCard;
    public bool isCardSelected = false;
    public Suit currentSuit;
    private bool isInPlay;
    [SerializeField] GameObject Club;
    [SerializeField] GameObject Diamond;
    [SerializeField] GameObject Heart;
    [SerializeField] GameObject Spade;

    private bool isFlipped = false;
    private float flipDuration = 0.5f;
    private float slideDuration = 0.5f;
    private float slideDistance = 5f;
    private Vector3 cardOriginPosition;
    private Quaternion cardOriginRotation;

    private void Start()
    {
        SetSuit(Suit.Heart);
        cardOriginPosition = transform.position;
        cardOriginRotation = transform.rotation;
        if (!isPlayersCard)
            FlipCard();
    }

    private void Update()
    {
        if (isCardSelected)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FlipCard();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                SetSuit(Suit.Club);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                SetSuit(Suit.Diamond);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                SetSuit(Suit.Heart);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SetSuit(Suit.Spade);
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                PlayCard();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                CardManager.Instance.RandomizeAllCards();
            }
        }

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

    public void ResetCard()
    {
        transform.position = cardOriginPosition;
        transform.rotation = cardOriginRotation;
    }

    public void SetSuit(Suit suit)
    {
        currentSuit = suit;
        UpdateActiveSuit();
    }

    private void PlayCard()
    {
        if (!isInPlay)
        {
            isInPlay = true;
            StartCoroutine(PlayCardCoroutine(isInPlay));
        }
        else
        {
            isInPlay = false;
            StartCoroutine(PlayCardCoroutine(isInPlay));
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
            transform.position = startPosition;
        }
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
                Club.SetActive(true);
                break;
            case Suit.Diamond:
                Diamond.SetActive(true);
                break;
            case Suit.Heart:
                Heart.SetActive(true);
                break;
            case Suit.Spade:
                Spade.SetActive(true);
                break;
        }
    }
}
