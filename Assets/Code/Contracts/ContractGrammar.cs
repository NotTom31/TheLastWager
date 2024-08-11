using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContractGrammar
{
    public enum Nonterminal
    {
        Base,
        PersistentEffect,
        ImmediateEffect,
        Event,
        Formation,
        Location,
        Player,
        Suit,
        Operator,
        Number
    }



    public static readonly Dictionary<Nonterminal, List<List<Symbol>>> GRAMMAR = new Dictionary<Nonterminal, List<List<Symbol>>>()
    {
        { Nonterminal.Base, new List<List<Symbol>>() {
            new List<Symbol>() {
                new Symbol(Nonterminal.PersistentEffect) },
            new List<Symbol>() {
                new Symbol("Whenever "),
                new Symbol(Nonterminal.Event),
                new Symbol(", "),
                new Symbol(Nonterminal.PersistentEffect),
                new Symbol(" for the rest of this hand") },
            new List<Symbol>() {
                new Symbol("The first time "),
                new Symbol(Nonterminal.Event),
                new Symbol(" each hand, "),
                new Symbol(Nonterminal.PersistentEffect),
                new Symbol(" for the rest of that hand") },
            new List<Symbol>() {
                new Symbol("Whenever "),
                new Symbol(Nonterminal.Event),
                new Symbol(", "),
                new Symbol(Nonterminal.ImmediateEffect) },
            new List<Symbol>() {
                new Symbol("The first time "),
                new Symbol(Nonterminal.Event),
                new Symbol(" each hand, "),
                new Symbol(Nonterminal.PersistentEffect),
                new Symbol(" for the rest of this hand") },
            new List<Symbol>() {
                new Symbol("Until you accept another contract, "),
                new Symbol(Nonterminal.PersistentEffect) }
        }},
        { Nonterminal.PersistentEffect, new List<List<Symbol>>() {
            new List<Symbol>() {
                new Symbol(Nonterminal.Suit),
                new Symbol(" is worth "),
                new Symbol(Nonterminal.Operator),
                new Symbol(Nonterminal.Number),
                new Symbol(" points") },
            new List<Symbol>() {
                new Symbol(Nonterminal.Suit),
                new Symbol(" is worth "),
                new Symbol(Nonterminal.Operator),
                new Symbol(Nonterminal.Number),
                new Symbol(" points for each "),
                new Symbol(Nonterminal.Formation),
                new Symbol(" in "),
                new Symbol(Nonterminal.Location) }
        }},
        { Nonterminal.ImmediateEffect, new List<List<Symbol>>() {
            new List<Symbol>() {
                new Symbol(Nonterminal.Player),
                new Symbol(" gets "),
                new Symbol(Nonterminal.Number),
                new Symbol(" points") },
            new List<Symbol>() {
                new Symbol(Nonterminal.Player),
                new Symbol(" gets "),
                new Symbol(Nonterminal.Number),
                new Symbol(" points for each "),
                new Symbol(Nonterminal.Formation),
                new Symbol(" in "),
                new Symbol(Nonterminal.Location) }
        }},
        { Nonterminal.Event, new List<List<Symbol>>() {
            new List<Symbol>() {
                new Symbol(Nonterminal.Player),
                new Symbol(" plays a "),
                new Symbol(Nonterminal.Suit) },
            new List<Symbol>() {
                new Symbol(Nonterminal.Player),
                new Symbol(" plays a "),
                new Symbol(Nonterminal.Suit),
                new Symbol(" followed by a "),
                new Symbol(Nonterminal.Suit) },
            new List<Symbol>() {
                new Symbol(Nonterminal.Player),
                new Symbol(" plays a "),
                new Symbol(Nonterminal.Suit),
                new Symbol(" followed by "),
                new Symbol(Nonterminal.Player),
                new Symbol(" playing a "),
                new Symbol(Nonterminal.Suit) }
        }},
        { Nonterminal.Formation, new List<List<Symbol>>() {
            new List<Symbol>() {
                new Symbol(Nonterminal.Suit) },
            new List<Symbol>() {
                new Symbol("Collection of "),
                new Symbol(Nonterminal.Number),
                new Symbol(Nonterminal.Suit),
                new Symbol("s") },
            new List<Symbol>() {
                new Symbol("Pairing of "),
                new Symbol(Nonterminal.Suit),
                new Symbol(" and "),
                new Symbol(Nonterminal.Suit) }
        }},
        { Nonterminal.Location, new List<List<Symbol>>() {
            new List<Symbol>() {
                new Symbol(Nonterminal.Player),
                new Symbol("'s hand") },
            new List<Symbol>() {
                new Symbol(Nonterminal.Player),
                new Symbol("'s side of the table") },
            new List<Symbol>() {
                new Symbol("the table") },
        }},
        { Nonterminal.Player, new List<List<Symbol>>() {
            new List<Symbol>() { new Symbol("you") },
            new List<Symbol>() { new Symbol("opponent") }
        }},
        { Nonterminal.Suit, new List<List<Symbol>>() {
            new List<Symbol>() { new Symbol("Spade") },
            new List<Symbol>() { new Symbol("Diamond") },
            new List<Symbol>() { new Symbol("Club") },
            new List<Symbol>() { new Symbol("Heart") }
        }},
        { Nonterminal.Operator, new List<List<Symbol>>() {
            new List<Symbol>() { new Symbol("Add") },
            new List<Symbol>() { new Symbol("Multiply") }
        }},
        { Nonterminal.Number, new List<List<Symbol>>() {
            new List<Symbol>() { new Symbol("-2") },
            new List<Symbol>() { new Symbol("-1") },
            new List<Symbol>() { new Symbol("0.5")},
            new List<Symbol>() { new Symbol("0")},
            new List<Symbol>() { new Symbol("1") },
            new List<Symbol>() { new Symbol("1.5")},
            new List<Symbol>() { new Symbol("2") },
            new List<Symbol>() { new Symbol("3") },
            new List<Symbol>() { new Symbol("4") },
            new List<Symbol>() { new Symbol("5") }
        }},
    };

    //should match the numbers above this
    public static readonly float[] NUMBER_LOOKUP = new float[] { -2f, -1f, 0.5f, 0f, 1f, 1.5f, 2f, 3f, 4f, 5f};
}
