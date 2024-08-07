using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
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



        after1stdrawbeforecontract();

        GetComponent<TextMeshProUGUI>().text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && queue.Count > 0)
        {
            GetComponent<TextMeshProUGUI>().text = queue[0];
            queue.RemoveAt(0);
        }
    }
    
    void start_diag(List<string> diag)
    {
        diag.Add("");
        queue.AddRange(diag);
    }


    public void after1stdrawbeforecontract()
    {
        start_diag(dialog1);
    }
    public void whenthe1stcontractsarepresented()
    {
        start_diag(dialog2);
    }
    public void rightbefore1stbet()
    {
        start_diag(dialog3);
    }
    public void rightbefore1sthandisplayed()
    {
        start_diag(dialog4);
    }
    public void enemy1stturn()
    {
        start_diag(dialog5);
    }
    public void onlyiftheplayerwins()
    {
        start_diag(dialog6);
    }
    public void onlyiftheplayerloses()
    {
        start_diag(dialog7);
    }
    public void rightbefore2nddraw()
    {
        start_diag(dialog8);
    }
    public void after2ndcontractdecition()
    {
        start_diag(dialog9);
    }
    public void rightbeforethetimerstarts()
    {
        start_diag(dialog10);
    }

}
