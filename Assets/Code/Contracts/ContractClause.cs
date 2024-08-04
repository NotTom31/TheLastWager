using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ContractGrammar;

//handles the rule-setting of a specific clause in a contract; NVM MOVE THAT LOGIC INTO A CONTRACT MANAGER?
public class ContractClause : MonoBehaviour
{
    SymbolNode baseNode;
    public string blurb { get; private set; }

    [SerializeField] int[] indices;
    
    private void Awake()
    {
        if (indices.Length != 0)
        {
            ContractGenerator cg = new ContractGenerator();
            baseNode = cg.NewNodeByIndex(indices);
        }
    }

    private void Start()
    {
        if (baseNode != null)
        {
            RigBaseNode(baseNode);
            blurb = baseNode.NodeAsSentence();
            Debug.Log(blurb);
        }
    }

    public void SetBaseNode(SymbolNode sn)
    {
        baseNode = sn;
    }

    //returns true if this node does have this symbol
    private bool VerifyNonterminal(SymbolNode node, Nonterminal nt)
    {
        return !node.GetSymbol().isString && node.GetSymbol().nt == nt;
    }

    //call this function to activate the rules logic for this clause
    private void RigBaseNode(SymbolNode node)
    {
        if (!VerifyNonterminal(node, Nonterminal.Base))
        {
            Debug.Log("ERROR: Base node expected.");
            return;
        }

        switch (node.formulaID)
        {
            case 0: //[PERSISTENT EFFECT]
                RigPersistentEffectNode(node.GetChildren()[0]);
                break;
        }
    }

    private void RigPersistentEffectNode(SymbolNode node)
    {
        if (!VerifyNonterminal(node, Nonterminal.PersistentEffect))
        {
            Debug.Log("ERROR: PersistentEffect node expected.");
            return;
        }

        List<SymbolNode> children = node.GetChildren();

        switch (node.formulaID)
        {
            case 0: //[SUIT] is worth [OPERATOR][X] points
                mathConstant = EvaluateNumberNode(children[3]);
                if (children[2].formulaID == 0) //addition
                {
                    switch (EvaluateSuitNode(children[0]))
                    {
                        case Suit.Spade:
                            ContractsManager.Instance.OnSpadesAddition += AddConstant;
                            break;
                        case Suit.Diamond:
                            ContractsManager.Instance.OnDiamondsAddition += AddConstant;
                            break;
                        case Suit.Club:
                            ContractsManager.Instance.OnClubsAddition += AddConstant;
                            break;
                        case Suit.Heart:
                            ContractsManager.Instance.OnHeartsAddition += AddConstant;
                            break;
                    }
                }
                else //multiplication
                {
                    switch (EvaluateSuitNode(children[0]))
                    {
                        case Suit.Spade:
                            ContractsManager.Instance.OnSpadesMultiplication += MultiplyConstant;
                            break;
                        case Suit.Diamond:
                            ContractsManager.Instance.OnDiamondsMultiplication += MultiplyConstant;
                            break;
                        case Suit.Club:
                            ContractsManager.Instance.OnClubsMultiplication += MultiplyConstant;
                            break;
                        case Suit.Heart:
                            ContractsManager.Instance.OnHeartsMultiplication += MultiplyConstant;
                            break;
                    }
                }
                break;
        }
    }

    private float mathConstant;
    public void AddConstant()
    {
        ContractsManager.Instance.pointReward += mathConstant;
    }
    public void MultiplyConstant()
    {
        ContractsManager.Instance.pointReward *= mathConstant;
    }
    

    private float EvaluateNumberNode(SymbolNode node)
    {
        if (!VerifyNonterminal(node, Nonterminal.Number))
        {
            Debug.Log("ERROR: Number node expected.");
            return -1f;
        }
        return NUMBER_LOOKUP[node.formulaID];
    }

    private Suit EvaluateSuitNode(SymbolNode node)
    {
        if (!VerifyNonterminal(node, Nonterminal.Suit))
        {
            Debug.Log("ERROR: Suit node expected.");
            return Suit.Spade;
        }
        Suit[] matrix = { Suit.Spade, Suit.Diamond, Suit.Club, Suit.Heart };
        return matrix[node.formulaID];
    }

    private int EvaluateFormationsInLocation(SymbolNode form, SymbolNode loc)
    {
        if (!VerifyNonterminal(form, Nonterminal.Formation) || !VerifyNonterminal(loc, Nonterminal.Location))
        {
            Debug.Log("ERROR: Formation node and Location node expected in that order.");
            return -1;
        }

        Dictionary<Suit, int> suitCounts = new Dictionary<Suit, int>()
        {
            { Suit.Spade, 0 },
            { Suit.Diamond, 0 },
            { Suit.Club, 0 },
            { Suit.Heart, 0 }
        };

        return 0;
    }
}
