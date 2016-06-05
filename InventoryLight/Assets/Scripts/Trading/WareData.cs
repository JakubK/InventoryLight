using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class WareData
{
    public int WareID { get; set; }
    public int Price { get; set; }
    public string CurrencyName { get; set; }

    public WareData()
    {
        CurrencyName = string.Empty;
    }
}