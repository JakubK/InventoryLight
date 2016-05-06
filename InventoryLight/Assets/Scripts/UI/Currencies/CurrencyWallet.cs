using UnityEngine;
using System.Collections;
using Assets.Scripts.Currencies;
using System.Collections.Generic;
using UnityEngine.Events;
using Assets.Scripts.Items;

public class CurrencyWallet : MonoBehaviour 
{
    public List<CurrencyData> CurrenciesData;

    public ItemDatabase database;

    public bool AutoConvertable = true;

    void Start()
    {
        CurrenciesData = new List<CurrencyData>();

        CurrenciesData.Add(new CurrencyData("Cents", 0));
        CurrenciesData.Add(new CurrencyData("Dollars", 0));
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

        if (addition)
        {
            if (cur != null)
            {
                bool isOk = true;
                for (int i = 0; i < currency.Dependencies.Count; i++)
                {
                    if (cur.Amount >= currency.Dependencies[i].FirstCurrencyCount)
                    {
                        isOk = false;
                        var secondCur = CurrenciesData.Find(x => x.Name == currency.Dependencies[i].SecondCurrency);

                        cur.Amount -= currency.Dependencies[i].FirstCurrencyCount;

                        secondCur.Amount += currency.Dependencies[i].SecondCurrencyCount;
                    }
                }

                if (isOk == false)
                {
                    AdjustCurrency(currency);
                }
            }
        }
        else
        {
            if (cur != null)
            {
                    bool result = false;
                    CurrencyDependency targetDependency = null;
                    Currency targetCurrency = null;

                    if (cur.Amount < 0)
                    {
                        foreach (var i in database.Currencies)
                        {
                            foreach (var d in i.Dependencies)
                            {
                                if (d.SecondCurrency == "Copper" && d.SecondCurrencyCount == 1)
                                {
                                    result = true;
                                    targetDependency = d;
                                    targetCurrency = i;
                                    break;
                                }
                            }
                            if (result == true)
                            {
                                break;
                            }
                        }

                        if (targetDependency != null)
                        {
                           var delta = CurrenciesData.Find(x => x.Name == currency.Name).Amount;
                           AddCurency(targetCurrency,Mathf.Abs(delta));
                           AdjustCurrency(currency, true);
                           AdjustCurrency(currency, false);

                           AdjustCurrency(targetCurrency, true);
                           AdjustCurrency(targetCurrency, false);

                        }
                    }
            }
        }
    }
}
