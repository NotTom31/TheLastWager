using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ContractGrammar;

public class Symbol
{
    public bool isString { get; private set; }
    public string str { get; private set; }
    public Nonterminal nt { get; private set; }
    public List<float> probabilityMatrix = new List<float>();

    public Symbol(string s)
    {
        isString = true;
        str = s;
        nt = Nonterminal.Base;
    }

    public Symbol(Nonterminal n, List<float> matrix = null)
    {
        isString = false;
        str = "";
        nt = n;
        /*
        if (matrix == null)
        {
            int size = GRAMMAR[nt].Count;
            float prob = 1.0f / size;
            probabilityMatrix = new List<float>();
            for (int i = 0; i < size; i++)
            {
                probabilityMatrix.Add(prob);
            }
        }
        else
            probabilityMatrix = matrix;
            */
    }
}
