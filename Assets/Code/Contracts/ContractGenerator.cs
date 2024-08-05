using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ContractGrammar;

//Creates a contract
public class ContractGenerator
{
    public SymbolNode NewNodeByIndex(params int[] indices)
    {
        SymbolNode n = new SymbolNode(new Symbol(Nonterminal.Base));
        n.SetChildren(MakeChildrenByIndex(n, indices).Item1);
        //Debug.Log(n.NodeAsSentence());
        return n;
    }

    (List<SymbolNode>, int[]) MakeChildrenByIndex(SymbolNode root, params int[] indices)
    {
        List<SymbolNode> result = new List<SymbolNode>();
        Symbol r = root.GetSymbol();
        if (r.isString)
            return (result, indices);
        List<List<Symbol>> possibilities = GRAMMAR[r.nt];
        int id = indices[0];
        indices = AllButFirst(indices);
        root.formulaID = id;
        List<Symbol> model = possibilities[id];
        foreach (Symbol c in model)
        {
            SymbolNode node = new SymbolNode(c);
            (List<SymbolNode>, int[]) recursiveResults = MakeChildrenByIndex(node, indices);
            node.SetChildren(recursiveResults.Item1);
            indices = recursiveResults.Item2;
            result.Add(node);
        }
        return (result, indices);
    }

    public int[] AllButFirst(int[] arr)
    {
        if (arr.Length == 1)
            return new int[0];
        List<int> temp = new List<int>();
        for (int i = 1; i < arr.Length; i++)
            temp.Add(arr[i]);
        return temp.ToArray();
    }

    public void MakeRandomAndPrint()
    {
        SymbolNode b = new SymbolNode(new Symbol(Nonterminal.PersistentEffect));
        b.SetChildren(GenerateRandomChildren(b));
        Debug.Log(b.NodeAsSentence());
    }

    List<SymbolNode> GenerateRandomChildren(SymbolNode root)
    {
        List<SymbolNode> result = new List<SymbolNode>();
        Symbol r = root.GetSymbol();
        if (r.isString)
            return result;
        List<List<Symbol>> possibilities = GRAMMAR[r.nt];
        //TODO: refine algorithm so this isn't random. Add some kind of complexity budget
        int id = Random.Range(0, possibilities.Count);
        root.formulaID = id;
        List<Symbol> model = possibilities[id];
        foreach (Symbol c in model)
        {
            SymbolNode node = new SymbolNode(c);
            node.SetChildren(GenerateRandomChildren(node));
            result.Add(node);
        }
        return result;
    }

    public string IntArrayToString(int[] arr)
    {
        string str = "[";
        for (int i = 0; i < arr.Length; i++)
        {
            str += arr[i].ToString();
            if (i + 1 != arr.Length)
                str += ",";
        }
        str += "]";
        return str;
    }

    public static List<int[]> PRESET_CLAUSE_CODES = new List<int[]>()
    {
        /*
        cg.NewNodeByIndex(0, 0, 0, 0, 4); //spades are worth +1
        cg.NewNodeByIndex(0, 0, 1, 0, 7); //diamonds are worth +3
        cg.NewNodeByIndex(0, 0, 2, 0, 0); //clubs are worth -2
        cg.NewNodeByIndex(0, 0, 3, 0, 4); //hearts are worth +1 */

        //set 1
        new int[] { 0, 0, 0, 0, 4 }, //spades are worth +1
        new int[] { 0, 0, 1, 0, 7 }, //diamonds are worth +3
        new int[] { 0, 0, 2, 0, 0 }, //clubs are worth -2
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1

        new int[] { 0, 0, 0, 0, 0 }, //spades are worth -2
        new int[] { 0, 0, 1, 0, 6 }, //diamonds are worth +2
        new int[] { 0, 0, 2, 0, 4 }, //clubs are worth +1
        new int[] { 0, 0, 3, 0, 7 }, //hearts are worth +3

        new int[] { 0, 0, 0, 0, 6 }, //spades are worth +2
        new int[] { 0, 0, 1, 0, 4 }, //diamonds are worth +1
        new int[] { 0, 0, 2, 0, 7 }, //clubs are worth +3
        new int[] { 0, 0, 3, 0, 1 }, //hearts are worth -1


        //set 2
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points


        //set 3
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 4
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 5
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 6
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 7
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 8
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 9
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 10
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 11
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

        //set 13
        new int[] { 0, 0, 3, 0, 4 }, //hearts are worth +1
        new int[] { 3, 0, 0, 0, 1, 0, 4, 0, 3, 0, 0 }, 
        //when you play a spade, gain a point for every Heart in your hand.
        new int[] { 3, 0, 0, 0, 0, 1, 6 },
        //when you play a spade, opponent gains 2 points

        new int[] { 0, 0, 1, 0, 0 }, //diamonds are worth -2
        new int[] { 3, 0, 0, 2, 1, 0, 4, 0, 1, 1, 1 }, 
        //when you play a Club, gain a point for every Diamond on your opponent's side of the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },
        //when you play a Heart, opponent gets -2 points

        new int[] { 0, 0, 2, 0, 6 }, //clubs are worth +2
        new int[] { 3, 0, 0, 3, 1, 0, 1, 0, 2, 2}, 
        //when you play a Heart, lose a point for every Club on the board.
        new int[] { 3, 0, 0, 3, 0, 1, 0 },

    };
}
