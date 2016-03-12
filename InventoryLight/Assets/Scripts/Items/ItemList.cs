using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Items
{
    public class ItemList : MonoBehaviour
    {
        public List<Item> Items;
        public ItemDatabase database;

        void Start()
        {
            Items = new List<Item>();
            
            Items.Add(new Item("Iron Sword","Two-handed iron sword"));
            Items.Add(new Item("Frostmourne", "Weapon made by The Lich King"));
            Items.Add(new Item("New Item3", "It's only an Item"));

            for (int i = 0; i < Items.Count; i++)
            {
               Items[i].ID = i;      
            }
        }

        void Update()
        {

        }
    }
}
