using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ContractGrammar;

public class SymbolNode
{
    Symbol thisSymbol;
    public int formulaID { get; set; }
    public List<float> probabilityMatrix { get; private set; } //probabilities of each possible formula being chosen
    List<SymbolNode> children;

    public SymbolNode(Symbol s)
    {
        thisSymbol = s;
        children = new List<SymbolNode>();
    }

    public string NodeAsSentence()
    {
        if (thisSymbol.isString)
            return thisSymbol.str;
        string str = "";
        foreach (SymbolNode sn in children)
        {
            str += sn.NodeAsSentence();
        }
        return str;
    }

    public void AddChild(SymbolNode n)
    {
        children.Add(n);
    }

    public void SetChildren(List<SymbolNode> list)
    {
        children = list;
    }

    public Symbol GetSymbol()
    {
        return thisSymbol;
    }

    public List<SymbolNode> GetChildren()
    {
        return children;
    }
}
