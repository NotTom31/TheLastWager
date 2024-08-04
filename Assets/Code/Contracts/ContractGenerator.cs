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
        Debug.Log(n.NodeAsSentence());
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
}
