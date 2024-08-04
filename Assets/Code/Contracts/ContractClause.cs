using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ContractGrammar;

//handles the rule-setting of a specific clause in a contract; NVM MOVE THAT LOGIC INTO A CONTRACT MANAGER?
public class ContractClause : MonoBehaviour
{
    SymbolNode baseNode;
    public string blurb { get; private set; }
    private int expiry; //goes down by 1 with each new contract. When it reaches 0, this is deactivated

    //TO IMPLEMENT PERSISTENT EFFECTS THAT
    //ONLY LAST FOR THE ROUND, INSTANTIATE AN INVISIBLE CONTRACT CLAUSE OF MODEL base -> persistent effect WITH AN EXPIRY OF 1 ROUND.

    [SerializeField] int[] indices;
    [SerializeField] Contract myContract;
    
    private void Awake()
    {
        if (indices.Length != 0)
        {
            ContractGenerator cg = new ContractGenerator();
            baseNode = cg.NewNodeByIndex(indices);
            blurb = baseNode.NodeAsSentence();
        }
    }

    //needs to be called before "Activate" to be effective.
    public void Init(int expry, int[] indx, Contract cont)
    {
        expiry = expry;
        indices = indx;
        ContractGenerator cg = new ContractGenerator();
        baseNode = cg.NewNodeByIndex(indx);
        myContract = cont;
    }

    public void Activate()
    {
        if (baseNode == null)
        {
            Debug.Log("ERROR: base node not assigned");
            return;
        }
        RigBaseNode(baseNode);
    }

    public void SetBaseNode(SymbolNode sn)
    {
        baseNode = sn;
        blurb = baseNode.NodeAsSentence();
    }

    public void AdvanceExpiry()
    {
        expiry -= 1;

        if (expiry <= 0)
        {
            if (myContract != null)
                myContract.NotifyExpiry(this);
            Destroy(this.gameObject);
        }
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

        ContractsManager.Instance.OnNewContract += AdvanceExpiry;

        switch (node.formulaID)
        {
            case 0: //[PERSISTENT EFFECT]
                RigPersistentEffectNode(node.GetChildren()[0]);
                break;
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
                //TODO
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
            case 1: //[SUIT] is worth [OPERATOR][X] points for each [FORMATION] in [LOCATION]
                mathConstant = EvaluateNumberNode(children[3]);
                formationNode = children[5];
                locationNode = children[7];
                if (children[2].formulaID == 0) //addition
                {
                    switch (EvaluateSuitNode(children[0]))
                    {
                        case Suit.Spade:
                            ContractsManager.Instance.OnSpadesAddition += AddVariableFormLoc;
                            break;
                        case Suit.Diamond:
                            ContractsManager.Instance.OnDiamondsAddition += AddVariableFormLoc;
                            break;
                        case Suit.Club:
                            ContractsManager.Instance.OnClubsAddition += AddVariableFormLoc;
                            break;
                        case Suit.Heart:
                            ContractsManager.Instance.OnHeartsAddition += AddVariableFormLoc;
                            break;
                    }
                }
                else //multiplication
                {
                    switch (EvaluateSuitNode(children[0]))
                    {
                        case Suit.Spade:
                            ContractsManager.Instance.OnSpadesMultiplication += MultiplyVariableFormLoc;
                            break;
                        case Suit.Diamond:
                            ContractsManager.Instance.OnDiamondsMultiplication += MultiplyVariableFormLoc;
                            break;
                        case Suit.Club:
                            ContractsManager.Instance.OnClubsMultiplication += MultiplyVariableFormLoc;
                            break;
                        case Suit.Heart:
                            ContractsManager.Instance.OnHeartsMultiplication += MultiplyVariableFormLoc;
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
    SymbolNode formationNode, locationNode;
    public void AddVariableFormLoc()
    {
        ContractsManager.Instance.pointReward += mathConstant * EvaluateFormationsInLocation(formationNode, locationNode);
    }
    public void MultiplyVariableFormLoc()
    {
        ContractsManager.Instance.pointReward *= mathConstant * EvaluateFormationsInLocation(formationNode, locationNode);
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

    private bool EvaluatePlayerNode(SymbolNode node)
    {
        if (!VerifyNonterminal(node, Nonterminal.Player))
        {
            Debug.Log("ERROR: Player node expected.");
            return false;
        }
        return node.formulaID == 0; //returns true if referring to user, false if devil
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

        List<Card> cards = new List<Card>();
        List<SymbolNode> locChildren = loc.GetChildren();
        switch (loc.formulaID)
        {
            case 0: //[PLAYER]'s hand
                if (EvaluatePlayerNode(locChildren[0])) //user
                    cards = new List<Card>(CardManager.Instance.playerHandCards);
                else //devil
                    cards = new List<Card>(CardManager.Instance.devilHandCards);
                break;
            case 1: //[PLAYER]'s side of the table
                if (EvaluatePlayerNode(locChildren[0])) //user
                    cards = new List<Card>(CardManager.Instance.playerTableCards);
                else //devil
                    cards = new List<Card>(CardManager.Instance.devilTableCards);
                break;
            case 2: //the table
                cards = new List<Card>(CardManager.Instance.playerTableCards);
                cards.AddRange(CardManager.Instance.devilTableCards);
                break;
        }
        foreach (Card c in cards)
            suitCounts[c.currentSuit] += 1;

        int count = 0;
        List<SymbolNode> formChildren = form.GetChildren();
        switch (form.formulaID)
        {
            case 0: //[SUIT]
                count = suitCounts[EvaluateSuitNode(formChildren[0])];
                break;
            case 1: //Collection of [X] [SUIT]s
                count = suitCounts[EvaluateSuitNode(formChildren[2])] / (int)EvaluateNumberNode(formChildren[1]);
                break;
            case 2: //Pairing of [SUIT] and [SUIT]
                count = Mathf.Min(suitCounts[EvaluateSuitNode(formChildren[1])], suitCounts[EvaluateSuitNode(formChildren[3])]);
                break;
        }
        return count;
    }

    private void TriggerImmediateEffect(SymbolNode node)
    {
        if (!VerifyNonterminal(node, Nonterminal.ImmediateEffect))
        {
            Debug.Log("ERROR: ImmediateEffect node expected.");
            return;
        }

        List<SymbolNode> children = node.GetChildren();
        switch (node.formulaID)
        {
            case 0: //[PLAYER] gains [X] points
                ContractsManager.Instance.AddPoints((int)EvaluateNumberNode(children[2]), EvaluatePlayerNode(children[0]));
                break;
            case 1: //[PLAYER] gains [X] points for each [FORMATION] in [LOCATION]
                ContractsManager.Instance.AddPoints((int)EvaluateNumberNode(children[2]) * EvaluateFormationsInLocation(children[4], children[6]), EvaluatePlayerNode(children[0]));
                break;
        }
    }
}
