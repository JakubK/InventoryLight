using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace Assets.Scripts.UI.Crafting
{
    public class OutputSlot : MonoBehaviour
    {
        protected List<InputSlot> InputSlots;
        ItemDatabase _database;

        void Start()
        {
            _database = transform.parent.GetComponent<CraftingManager>().database;
        }

        public void Call(int outputID)
        {
            GameObject itemInstance = Instantiate(transform.parent.GetComponent<CraftingManager>().itemPrefab.gameObject);
            itemInstance.name = "Item";

            itemInstance.transform.SetParent(transform);
            itemInstance.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            ItemData id = itemInstance.AddComponent<ItemData>();
            id.HoldedItem = _database.ItemByID(outputID);
            id.GetComponent<Image>().sprite = id.HoldedItem.Icon;
        }
    }
}
