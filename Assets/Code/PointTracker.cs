using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointTracker : MonoBehaviour
{
    public int DevilPoints { get; private set; }
    public int UserPoints { get; private set; }
    [SerializeField] TextMeshProUGUI devilTextBox;
    [SerializeField] TextMeshProUGUI userTextBox;

    private void Awake()
    {
        UpdateGUI();
    }

    private void UpdateGUI()
    {
        devilTextBox.text = DevilPoints.ToString();
        userTextBox.text = UserPoints.ToString();
    }

    public void AddPoints(int amt, bool toPlayer)
    {
        if (toPlayer)
            UserPoints += amt;
        else
            DevilPoints += amt;
        UpdateGUI();
    }

    public void ResetPoints()
    {
        SetDevilPoints(0);
        SetUserPoints(0);
        UpdateGUI();
    }

    public int ComparePoints()
    {
        if(UserPoints > DevilPoints)
        {
            Debug.Log("win");
            ResetPoints();
            return 0;
        }
        else if (UserPoints < DevilPoints)
        {
            Debug.Log("lose");
            ResetPoints();
            return 1;
        }
        else
        {
            Debug.Log("tie");
            ResetPoints();
            return 2;
        }
    }

    public void SetUserPoints(int i)
    {
        UserPoints = i;
        UpdateGUI();
    }

    public void SetDevilPoints(int i)
    {
        DevilPoints = i;
        UpdateGUI();
    }
}
