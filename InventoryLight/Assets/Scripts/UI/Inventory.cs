using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Items;
using Assets.Scripts.Serialization;
using UnityEngine.UI;
using Assets.Scripts.Crafting;

namespace Assets.Scripts.UI
{
    public class Inventory : MonoBehaviour
    {
        public ItemDatabase ItemDatabase;

        [SerializeField] private Transform slotPanel;

        [SerializeField] private Transform slotPrefab;

        [SerializeField] private Transform itemPrefab;

        [SerializeField] private int MaxSize;

        [SerializeField]
        private string ContainerPath;

        [SerializeField]
        private bool SaveOnClose;

        public Transform Tooltip;

        public bool DragAndDropEnabled = true;

        public List<Transform> SlotList;
        public List<ItemData> ItemList;
        private List<ItemDataParams> paramses; 

        public int OnUseClickCount;

        CurrencyWallet wallet;

        void Start()
        {
            SlotList = new List<Transform>();
            ItemList = new List<ItemData>();
            FillWithSlots();
            RestoreLastSession();

            wallet = GetComponent<CurrencyWallet>();
        }

        void OnApplicationQuit()
        {
            if (SaveOnClose)
            {
                SaveSession();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                wallet.AddCurency(ItemDatabase.CurrencyByName("Cents"), 10);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                wallet.RemoveCurrency(ItemDatabase.CurrencyByName("Cents"), 10);  
            }
            else if (Input.GetKeyDown(KeyCode.O))
            {
                print(wallet.ByNameCurrencyData("Cents").Amount.ToString() + " cents " + wallet.ByNameCurrencyData("Dollars").Amount.ToString() + " dollars.");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                ClearInventory();
            }
        }

        public void SaveSession()
        {
            ItemCollectionSerializer ics = new ItemCollectionSerializer(ItemList);
            ics.Save(ContainerPath);
        }

        public void ClearInventory()
        {
            ItemList.Clear();
            foreach (Transform t in SlotList)
            {
                foreach (Transform i in t)
                {
                    Destroy(i.gameObject);
                }
            }
        }

		public bool canAffordRecipe(int ID)
		{
			bool result = false;

			Recipe rec = ItemDatabase.RecipeByName (ItemDatabase.ItemByID (ID).Name);

			Dictionary<int,int> IdsDictionary = new Dictionary<int, int> ();

			foreach (Item i in rec.RequiredData)
			{
				if (!IdsDictionary.ContainsKey(i.ID))
				{
					int count = 0;
					for (int j = 0; j < rec.RequiredData.Count; j++)
					{
						if (rec.RequiredData[j].ID == i.ID)
						{
							count++;
						}
					}
					IdsDictionary.Add(i.ID, count);
				}
			}

			foreach (var i in IdsDictionary) 
			{
				if (i.Value > GetItemCount (i.Key)) {
					result = false;
					break;
				} else {
					result = true;
				}
			}
			return result;
		}

        public void RestoreLastSession()
        {
            ItemCollectionSerializer ics = new ItemCollectionSerializer();
            paramses = ics.Load(ContainerPath);
            ItemList.Clear();
            foreach (Transform t in SlotList)
            {
                foreach (Transform i in t)
                {
                    Destroy(i.gameObject);
                }
            }

            foreach (ItemDataParams i in paramses)
            {
                AddItem(i.ID, i.Amount,i.slotID);
            }
          
        }

        void OnDisable()
        {
            if (SaveOnClose)
            {
                SaveSession();
            }
        }

		public int GetItemCount(int ID)
		{
			int result = 0;
			foreach (var data in ItemList) {
				if (data.HoldedItem.ID == ID) {
					result += data.Amount;
				}
			}

			return result;
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
                    
                    Slot slot = slotInstance.GetComponent<Slot>();
                    slot.ID = i;

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

        public void AddItem(int ID, int Amount,int Slot)
        {
            GameObject itemInstance = Instantiate(itemPrefab).gameObject;
            itemInstance.name = "Item";

            itemInstance.transform.SetParent(SlotList[Slot].transform);
            itemInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            ItemData itemData = itemInstance.AddComponent<ItemData>();
            ItemList.Add(itemData);
            itemData.HoldedItem = ItemDatabase.ItemByID(ID);
            itemData.Amount = Amount;
            itemData.Slot = Slot;

            itemInstance.GetComponent<Image>().sprite = itemData.HoldedItem.Icon;
            if (Amount > 1)
            {
                itemInstance.transform.GetChild(0).GetComponent<Text>().text = Amount.ToString();
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
                    itemData.Slot = i;

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
}

