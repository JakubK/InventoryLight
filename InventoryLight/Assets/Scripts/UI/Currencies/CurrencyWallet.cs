using UnityEngine;
using System.Collections;
using Assets.Scripts.Currencies;
using System.Collections.Generic;
using UnityEngine.Events;
using Assets.Scripts.Items;
using UnityEngine.UI;

public class CurrencyWallet : MonoBehaviour 
{
    public List<CurrencyData> CurrenciesData;

    public ItemDatabase database;

    public bool AutoConvertable = true;

    [SerializeField]
    Transform WalletDisplayerTransform;

    Text WalletDisplayer;

    void Start()
    {
        CurrenciesData = new List<CurrencyData>();
        WalletDisplayer = WalletDisplayerTransform.GetComponent<Text>();
        //CurrenciesData.Add(new CurrencyData("Copper", 0));
        //CurrenciesData.Add(new CurrencyData("Silver", 0));
        //CurrenciesData.Add(new CurrencyData("Gold", 0));
        foreach (var i in database.Currencies)
        {
            CurrenciesData.Add(new CurrencyData(i.Name, 0));
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCurency(database.CurrencyByName("Copper"), 10);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            RemoveCurrency(database.CurrencyByName("Copper"), 10);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            print(ByNameCurrencyData("Gold").Amount.ToString() + " gold " + ByNameCurrencyData("Silver").Amount.ToString() + " silver" + ByNameCurrencyData("Copper").Amount.ToString() + " copper");
        }
    }

    public void RemoveCurrency(Currency currency,int count)
    {
        bool result = false;
        for (int i = 0; i < CurrenciesData.Count; i++)
        {
            if (CurrenciesData[i].Name == currency.Name)
            {
                CurrenciesData[i].Amount -= count;
                result = true;
                break;
            }
        }

        if (result == false)
        {
            CurrenciesData.Add(new CurrencyData(currency.Name, count));
        }

        if (AutoConvertable)
        {
            AdjustCurrency(currency,false);
        }
    }

    public CurrencyData ByNameCurrencyData(string name)
    {
        return CurrenciesData.Find(x => x.Name == name);
    }

    public void SetCurrency(Currency currency,int count)
    {
        CurrenciesData.Find(x => x.Name == currency.Name).Amount = count;
    }

    public void AddCurency(Currency currency, int count)
    {
        bool result = false;
        for (int i = 0; i < CurrenciesData.Count; i++)
        {
            if (CurrenciesData[i].Name == currency.Name)
            {
                CurrenciesData[i].Amount += count;
                result = true;
                break;
            }
        }

        if (result == false)
        {
            CurrenciesData.Add(new CurrencyData(currency.Name, count));
        }

        if (AutoConvertable)
        {
            AdjustCurrency(currency,true);
        }
    }

    public void AdjustCurrency(Currency currency,bool addition = true)
    {
        var cur = CurrenciesData.Find(x => x.Name == currency.Name);

        int result = 0;
        foreach (CurrencyDependency dependency in currency.Dependencies)
        {
            if (cur.Amount >= dependency.FirstCurrencyCount)
            {
                result = 1;

                RemoveCurrency(database.CurrencyByName(cur.Name), dependency.FirstCurrencyCount);
                var secondCur = CurrenciesData.Find(x => x.Name == dependency.SecondCurrency);
                AddCurency(database.CurrencyByName(secondCur.Name), dependency.SecondCurrencyCount);
                break;
            }
        }

        if (result == -1)
        {
            AdjustCurrency(currency, false);
        }
        else if (result == 1)
        {
            AdjustCurrency(currency, true);
        }

        WalletDisplayer.text = "Gold: " + ByNameCurrencyData("Gold").Amount + " Silver: " + ByNameCurrencyData("Silver").Amount + " Copper: " + ByNameCurrencyData("Copper").Amount;
    }
}
