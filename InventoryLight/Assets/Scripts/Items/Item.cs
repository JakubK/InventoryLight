using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Items
{
    [Serializable]
    public class Item
    {
        public int ID;
        public string Name;
        public string Description;

        public List<ItemProperty> ItemProperties;
        public ItemCategory itemCategory;

        public Item()
        {
        }

        public Item(string name,string description)
        {
            this.Name = name;
            this.Description = description;
        }

        public delegate void ItemAction();

        public event ItemAction OnUse;

        public void Use()
        {
            if (OnUse != null)
            {
                Debug.Log("You have used the " + Name);
                OnUse();
            }
        }
    }
}
