using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    // i dont care anyomre
    List<string> dialog1 = new List<string>();
    List<string> dialog2 = new List<string>();
    List<string> dialog3 = new List<string>();
    List<string> dialog4 = new List<string>();
    List<string> dialog5 = new List<string>();
    List<string> dialog6 = new List<string>();
    List<string> dialog7 = new List<string>();
    List<string> dialog8 = new List<string>();
    List<string> dialog9 = new List<string>();
    List<string> dialog10 = new List<string>();
    List<string> queue = new List<string>();

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] AudioSource audioSource;

    private int currentDialogue = 0;

    public static DialogueManager Instance;

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

    void Start()
    {
        dialog1.Add("Hello, Mortal.");
        dialog1.Add("I'm pleased to have finally met your acquaintance.");
        dialog1.Add("I am the Soul Proprietor.");
        dialog1.Add("-and i see you only have half of your soul left");
        dialog1.Add("Half a full soul, you can see it in pieces to your right.");
        dialog1.Add("I want to play a small game...");
        dialog1.Add("We play a game of cards for your last remaining essence of life...");
        dialog1.Add("...the last few fragments of your soul");

        dialog2.Add("You see, in my game,");
        dialog2.Add("the cards don't have any action on their own");
        dialog2.Add("It is YOU that determines what they do.");
        dialog2.Add("I have some... contracts that I need you to sign");
        dialog2.Add("Each has clauses that change what each of the suits do");
        dialog2.Add("The goal is to have a hand that wins more points than mine");
        dialog2.Add("Though, I must warn you- ");
        dialog2.Add("I've been playing this game...");
        dialog2.Add("...for longer than you humans have walked the earth.");
        dialog2.Add("Now choose. You must sign one.");

        dialog3.Add("Now you see your hand, you see the rules.");
        dialog3.Add("Now you must choose how much of your soul to risk.");
        dialog3.Add("This is your last chance at salvation.");

        dialog4.Add("Now we play!");
        dialog4.Add("One life changing card after another.");
        dialog4.Add("You go first. It won't matter anyways.");

        dialog5.Add("My turn.");

        dialog6.Add("You win.");
        dialog6.Add("Beginners luck, I suppose.");

        dialog7.Add("You lose.");
        dialog7.Add("Expected.");

        dialog8.Add("Now we draw again");
        dialog8.Add("I write more contracts, and you choose");
        dialog8.Add("These ones, they are more interesting.");
        dialog8.Add("Lets see what you choose");

        dialog9.Add("Interesting decision.");

        dialog10.Add("Well, this has been fun,");
        dialog10.Add("I think it's time to take things a little...");
        dialog10.Add("... more seriously.");





        //GetComponent<TextMeshProUGUI>().text = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NextDialogue()
    {
        if (queue.Count > 1)
        {
            text.text = queue[0];
            queue.RemoveAt(0);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        GameManager.Instance.FinishedDialogue(currentDialogue);
    }

    void StartDialogue(List<string> diag)
    {
        GameManager.Instance.SetState(GameState.DIALOGUE);
        MenuManager.Instance.OpenDialogue();
        diag.Add("");
        queue.AddRange(diag);
        NextDialogue();
    }


    public void after1stdrawbeforecontract()
    {
        currentDialogue = 1;
        StartDialogue(dialog1);
    }
    public void whenthe1stcontractsarepresented()
    {
        currentDialogue = 2;
        StartDialogue(dialog2);
    }
    public void rightbefore1stbet()
    {
        currentDialogue = 3;
        StartDialogue(dialog3);
    }
    public void rightbefore1sthandisplayed()
    {
        currentDialogue = 4;
        StartDialogue(dialog4);
    }
    public void enemy1stturn()
    {
        currentDialogue = 5;
        StartDialogue(dialog5);
    }
    public void onlyiftheplayerwins()
    {
        currentDialogue = 6;
        StartDialogue(dialog6);
    }
    public void onlyiftheplayerloses()
    {
        currentDialogue = 7;
        StartDialogue(dialog7);
    }
    public void rightbefore2nddraw()
    {
        currentDialogue = 8;
        StartDialogue(dialog8);
    }
    public void after2ndcontractdecition()
    {
        currentDialogue = 9;
        StartDialogue(dialog9);
    }
    public void rightbeforethetimerstarts()
    {
        currentDialogue = 10;
        StartDialogue(dialog10);
    }

}
