using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomEditor(typeof(UITrader))]
public class TradeEditor :  Editor {

    Vector2 scrollVector = new Vector2();
    bool showItems = false;
    bool showCurrencyData = false;
    WareData EditedWare;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        UITrader trader = (UITrader)target;

        if (GUILayout.Button("Add new Ware"))
        {
            showItems = !showItems;
        }
        if (showItems)
        {
            WareData ware = new WareData();

            GUILayout.Space(10);
            scrollVector = GUILayout.BeginScrollView(scrollVector,GUILayout.Height(100));
            foreach (var i in trader.database.ItemList)
            {
                if (GUILayout.Button(i.Name.ToString()))
                {
                    bool found = false;
                    if (trader.Wares == null)
                    {
                        trader.Wares = new System.Collections.Generic.List<WareData>();
                    }
                    foreach (var j in trader.Wares)
                    {
                        if (j.WareID == i.ID)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        ware.WareID = i.ID;

                        EditedWare = null;
                        EditedWare = ware;

                        showItems = false;
                        showCurrencyData = true;

                        trader.Wares.Add(ware);
                    }
                }
            }
            GUILayout.EndScrollView();
        }
        if (showCurrencyData)
        {
            try
            {
                GUILayout.Label("Currency Name");
                EditedWare.CurrencyName = GUILayout.TextField(EditedWare.CurrencyName);

                GUILayout.Label("Ware Price");
                EditedWare.Price = int.Parse(GUILayout.TextField(EditedWare.Price.ToString()));

                if(GUILayout.Button("Finish"))
                {
                    showCurrencyData = false;
                }
            }
            catch (Exception ex)
            {

            }
        }


        foreach (var i in trader.Wares)
        {
            GUILayout.Label(i.CurrencyName.ToString());
        }
    }
}
