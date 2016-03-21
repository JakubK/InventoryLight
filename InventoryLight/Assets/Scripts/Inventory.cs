using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Items;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

	// Use this for initialization
    public ItemDatabase ItemDatabase;

    [SerializeField]
    private Transform slotPanel;

    [SerializeField]
    private Transform slotPrefab;

    [SerializeField]
    private Transform itemPrefab;

    [SerializeField]
    private int MaxSize;

    public List<Transform> SlotList;
    public List<ItemData> ItemList;



	void Start () 
    {
        SlotList = new List<Transform>();
        ItemList = new List<ItemData>();

	    FillWithSlots();
	}
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetKeyDown(KeyCode.A))
	    {
	        AddItem(0);
	    }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            RemoveItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            AddItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            RemoveItem(1);
        }
        
	}

    private void FillWithSlots()
    {
       SlotList.Clear();
       ItemList.Clear();

       for (int i = 0; i < slotPanel.childCount; i++)
       {
          Destroy(slotPanel.GetChild(i).gameObject);
       }

       for (int i = 0; i < MaxSize; i++)
       {
           if (slotPrefab != null && itemPrefab != null)
           {
               GameObject slotInstance = Instantiate(slotPrefab).gameObject;
               slotInstance.name = "Slot";

               SlotList.Add(slotInstance.transform);
               slotInstance.transform.SetParent(slotPanel.transform);
           }
           else
           {
               throw new UnassignedReferenceException("slotPrefab or itemPrefab is not assigned");
           }
       }
       
    }

    public void ReinitializeSlots()
    {
        ItemList.Clear();
        for (int i = 0; i < SlotList.Count; i++)
        {
            if (SlotList[i].childCount != 0)
            {
                ItemList.Add(SlotList[i].GetChild(0).GetComponent<ItemData>());
            }
        }
    }

    public void RemoveItem(int ID)
    {
        ReinitializeSlots();
        for (int i = 0; i < ItemList.Count; i++)
        {
            if (ItemList[i].HoldedItem.ID == ID)
            {
                if (ItemList[i].Amount > 1)
                {
                    ItemList[i].Amount--;
                    if (ItemList[i].Amount > 1)
                    {
                        ItemList[i].transform.GetChild(0).GetComponent<Text>().text = ItemList[i].Amount.ToString();
                    }
                    else
                    {
                        ItemList[i].transform.GetChild(0).GetComponent<Text>().text = "";
                    }
                    break;
                }
                else
                {
                    Destroy(ItemList[i].gameObject);
                    ItemList.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void AddItem(int ID)
    {
        for (int i = 0; i < SlotList.Count; i++)
        {
            if (SlotList[i].childCount == 0)
            {
                GameObject itemInstance = Instantiate(itemPrefab).gameObject;
                itemInstance.name = "Item";

                itemInstance.transform.SetParent(SlotList[i].transform);
                itemInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                ItemData itemData = itemInstance.AddComponent<ItemData>();
                ItemList.Add(itemData);
                itemData.HoldedItem = ItemDatabase.ItemByID(ID);
                itemData.Amount = 1;

                itemInstance.GetComponent<Image>().sprite = itemData.HoldedItem.Icon;

                break;
            }
            else
            {
                if (SlotList[i].GetChild(0).GetComponent<ItemData>().HoldedItem.ID == ID)
                {
                    if (SlotList[i].GetChild(0).GetComponent<ItemData>().Amount <
                        SlotList[i].GetChild(0).GetComponent<ItemData>().HoldedItem.MaxStackCount)
                    {
                        SlotList[i].GetChild(0).GetComponent<ItemData>().Amount++;
                        if (SlotList[i].GetChild(0).GetComponent<ItemData>().Amount > 1)
                        {
                            SlotList[i].GetChild(0).GetChild(0).GetComponent<Text>().text =
                                SlotList[i].GetChild(0).GetComponent<ItemData>().Amount.ToString();
                        }
                        break;
                    }
                }
            }
        }
    }
}

