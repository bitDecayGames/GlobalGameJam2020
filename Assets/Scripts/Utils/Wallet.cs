using System;
using TMPro;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int Money;
    public TextMeshProUGUI TextMeshProUI;

    private void Update()
    {
        TextMeshProUI.text = $"Wallet: ${Money}";
    }

    public void AddMoney(int money)
    {
        Money += money;
    }

    public void SubtractMoney(int money)
    {
        Money -= money;
    }
    
}