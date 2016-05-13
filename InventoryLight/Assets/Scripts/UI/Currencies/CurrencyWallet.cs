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

        CurrenciesData.Add(new CurrencyData("Copper", 0));
        CurrenciesData.Add(new CurrencyData("Silver", 0));
        CurrenciesData.Add(new CurrencyData("Gold", 0));

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
            else if (cur.Amount < 0)
            {
                result = -1;

                Currency targetCurrency = null;
                CurrencyDependency targetDependency = null;

                foreach (Currency cu in database.Currencies)
                {
                    foreach (CurrencyDependency dep in cu.Dependencies)
                    {
                        if (dep.SecondCurrency == currency.Name && dep.SecondCurrencyCount == 1)
                        {
                            targetCurrency = cu; //niższa waluta
                            targetDependency = dep;
                            goto Rest;
                        }
                    }
                }
            Rest:

                if (targetCurrency != null)
                {
                    var secondCur = CurrenciesData.Find(x => x.Name == targetCurrency.Name); //niższa waluta
                    int delta = cur.Amount - secondCur.Amount;

                    int RemoveCoins = Mathf.Abs(delta) * targetDependency.FirstCurrencyCount;

                    RemoveCurrency(database.CurrencyByName(secondCur.Name), RemoveCoins);
                }


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
    }
}
