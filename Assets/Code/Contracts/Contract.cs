using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Collection of contract clauses. Access this script to get text for all relevant contract clauses
public class Contract : MonoBehaviour
{
    //initialization on prefab
    [SerializeField] int[] clause0Code;
    [SerializeField] int[] clause1Code;
    [SerializeField] int[] clause2Code;
    [SerializeField] int[] clause3Code;
    [SerializeField] int startingExpiry;

    [SerializeField] GameObject clausePrefab;

    ContractClause[] clauses;
    public string[] blurbs { get; private set; }
    public bool[] grayedMatrix { get; private set; } //keeps track of which blurbs are grayed out due to inactivity

    [SerializeField] bool InitOnAwake;
    [SerializeField] bool ActivateOnStart;
    private void Awake()
    {
        if (InitOnAwake)
        {
            InitializeClauses();
        }
    }
    private void Start()
    {
        if (ActivateOnStart)
        {
            ActivateClauses();
        }
    }

    public void InitializeClauses()
    {
        clauses = new ContractClause[4];
        List<int[]> lookup = new List<int[]>() { clause0Code, clause1Code, clause2Code, clause3Code };
        for (int i = 0; i < 4; i++)
        {
            if (lookup[i].Length == 0)
                continue;
            ContractClause clause = GameObject.Instantiate(clausePrefab, transform).GetComponent<ContractClause>();
            clause.Init(startingExpiry, lookup[i], this);
            clauses[i] = clause;
        }
        InitBlurbs();
        InitGrayedMatrix();
    }

    private void InitBlurbs()
    {
        blurbs = new string[4];
        for (int i = 0; i < 4; i++)
        {
            if (clauses[i] == null)
                blurbs[i] = "no clause";
            else
                blurbs[i] = clauses[i].blurb;
        }
    }

    private void InitGrayedMatrix()
    {
        grayedMatrix = new bool[4];
        //all default to false
    }

    //called by ContractClause class which should have this stored as its myContract
    public void NotifyExpiry(ContractClause caller)
    {
        int idx = System.Array.IndexOf(clauses, caller);
        grayedMatrix[idx] = true;
        clauses[idx] = null;

        if (AllGrayed())
            Debug.Log("NOW WE NEED TO DESTROY A CONTRACT");
    }

    private bool AllGrayed()
    {
        bool allgrayed = true;
        foreach (bool b in grayedMatrix)
            if (!b)
                allgrayed = false;
        return allgrayed;
    }

    //Call when the player accepts this contract, so that all the clauses now effect the rules
    public void ActivateClauses()
    {
        foreach (ContractClause cc in clauses)
        {
            if (cc == null)
                continue;
            cc.Activate();
        }
    }
}
