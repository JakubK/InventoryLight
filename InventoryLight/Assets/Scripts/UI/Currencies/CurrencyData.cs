using UnityEngine;
using System.Collections;
using Assets.Scripts.Currencies;
using System;

public class CurrencyData
{
    public string Name;
    public int Amount;

    public CurrencyData(string name, int amount)
    {
        this.Name = name;
        this.Amount = amount;
    }
}
