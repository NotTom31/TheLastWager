using System.Collections;
using TMPro;
using UnityEngine;

public class SoulTracking : MonoBehaviour
{
    public float Souls { get; private set; }

    [SerializeField] TextMeshProUGUI soulTextBox;

    private void Awake()
    {
        Souls = 50f;
        ResetSouls();
    }

    private void UpdateGUI()
    {
        soulTextBox.text = Souls.ToString("F0");
    }

    public void SetSoulCount(float c)
    {
        Souls = c;
        ResetSouls();
    }

    public void ResetSouls()
    {
        
        UpdateGUI();
    }
}
