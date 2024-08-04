using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles the rule-setting of a specific clause in a contract
public class ContractClause : MonoBehaviour
{
    SymbolNode baseNode;

    private void Awake()
    {
        ContractGenerator cg = new ContractGenerator();
        cg.NewNodeByIndex(0, 0, 0, 0, 4); //spades are worth +1
        cg.NewNodeByIndex(0, 0, 1, 0, 7); //diamonds are worth +3
        cg.NewNodeByIndex(0, 0, 2, 0, 0); //clubs are worth -2
        cg.NewNodeByIndex(0, 0, 3, 0, 4); //hearts are worth +1
    }

    public void SetBaseNode(SymbolNode sn)
    {
        baseNode = sn;
    }
}
