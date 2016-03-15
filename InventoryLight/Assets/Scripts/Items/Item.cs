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

        public Sprite Icon;
        public GameObject ItemPrefab;

        public List<ItemProperty> ItemProperties;
        public ItemCategory itemCategory;

        public int MaxStackCount;

        public int categoryChoiceID;

        public Item()
        {
        }

        public Item(string name,string description)
        {
            this.Name = name;
            this.Description = description;
            this.MaxStackCount = 1;
            this.categoryChoiceID = 0;
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
