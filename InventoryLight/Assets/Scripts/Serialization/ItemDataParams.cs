using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Items;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Serialization
{
    [Serializable]
    public class ItemDataParams
    {
        public int ID;
        public List<ItemProperty>Properties; 
        public int Amount;

        public int slotID;

        public ItemDataParams(ItemData data)
        {
            this.ID = data.HoldedItem.ID;
            this.slotID = data.Slot;

            Properties = new List<ItemProperty>();
            foreach (ItemProperty property in data.Properties )
            {
                Properties.Add(new ItemProperty(property.PropertyName,property.PropertyValue));
            }

            this.Amount = data.Amount;
        }
    }
}
