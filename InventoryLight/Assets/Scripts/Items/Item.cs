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

        public AudioClip OnGearAudioClip;
        public AudioClip OnUseAudioClip;

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

            ItemProperties = new List<ItemProperty>();
            itemCategory = new ItemCategory();
        }

        public delegate void ItemAction();

        public event ItemAction OnUse;
        public event ItemAction OnGear;

        public void Use()
        {
            if (OnUse != null)
            {
                OnUse();
            }
        }

        public void Gear()
        {
            if (OnUse != null)
            {
                OnGear();
            }
        }
    }
}
